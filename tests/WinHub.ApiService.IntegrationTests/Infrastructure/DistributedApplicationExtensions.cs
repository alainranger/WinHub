// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Cryptography;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

using WinHub.ApiService.IntegrationTests.Infrastructure;

namespace WinHub.ApiService.IntegrationTests.Infrastructure;

public static partial class DistributedApplicationExtensions
{
	/// <summary>
	/// Waits for the specified resource to reach the specified state.
	/// </summary>
	public static Task WaitForResource(this DistributedApplication app, string resourceName, string? targetState = null, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(app);

		targetState ??= KnownResourceStates.Running;
		var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

		return resourceNotificationService.WaitForResourceAsync(resourceName, targetState, cancellationToken);
	}

	/// <summary>
	/// Waits for all resources in the application to reach one of the specified states.
	/// </summary>
	/// <remarks>
	/// If <paramref name="targetStates"/> is null, the default states are <see cref="KnownResourceStates.Running"/> and <see cref="KnownResourceStates.Hidden"/>.
	/// </remarks>
	public static Task WaitForResources(this DistributedApplication app, IEnumerable<string>? targetStates = null, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(app);

		targetStates ??= [KnownResourceStates.Running, KnownResourceStates.Hidden];
		var applicationModel = app.Services.GetRequiredService<DistributedApplicationModel>();
		var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

		return Task.WhenAll(applicationModel.Resources.Select(r => resourceNotificationService.WaitForResourceAsync(r.Name, targetStates, cancellationToken)));
	}

	/// <summary>
	/// Creates an <see cref="HttpClient"/> configured to communicate with the specified resource.
	/// </summary>
	public static HttpClient CreateHttpClient(this DistributedApplication app, string resourceName, bool useHttpClientFactory)
		=> app.CreateHttpClient(resourceName, null, useHttpClientFactory);

	/// <summary>
	/// Creates an <see cref="HttpClient"/> configured to communicate with the specified resource.
	/// </summary>
	public static HttpClient CreateHttpClient(this DistributedApplication app, string resourceName, string? endpointName, bool useHttpClientFactory)
	{
		if (useHttpClientFactory)
			return app.CreateHttpClient(resourceName, endpointName);

		// Don't use the HttpClientFactory to create the HttpClient so, e.g., no resilience policies are applied
		var httpClient = new HttpClient
		{
			BaseAddress = app.GetEndpoint(resourceName, endpointName)
		};

		return httpClient;
	}

	/// <summary>
	/// Creates an <see cref="HttpClient"/> configured to communicate with the specified resource with custom configuration.
	/// </summary>
	public static HttpClient CreateHttpClient(this DistributedApplication app, string resourceName, string? endpointName, Action<IHttpClientBuilder> configure)
	{
		var services = new ServiceCollection()
			.AddHttpClient()
			.ConfigureHttpClientDefaults(configure)
			.BuildServiceProvider();
		var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();

		var httpClient = httpClientFactory.CreateClient();
		httpClient.BaseAddress = app.GetEndpoint(resourceName, endpointName);

		return httpClient;
	}

	/// <summary>
	/// Asserts that no errors were logged by the application or any of its resources.
	/// </summary>
	/// <remarks>
	/// Some resource types are excluded from this check because they tend to write to stderr for various non-error reasons.
	/// </remarks>
	/// <param name="app"></param>
	public static void EnsureNoErrorsLogged(this DistributedApplication app)
	{
		ArgumentNullException.ThrowIfNull(app);

		var environment = app.Services.GetRequiredService<IHostEnvironment>();
		var applicationModel = app.Services.GetRequiredService<DistributedApplicationModel>();
		var assertableResourceLogNames = applicationModel.Resources.Where(ShouldAssertErrorsForResource).Select(r => $"{environment.ApplicationName}.Resources.{r.Name}").ToList();

		var (appHostlogs, resourceLogs) = app.GetLogs();

		Assert.DoesNotContain(appHostlogs, log => log.Level >= LogLevel.Error);
		Assert.DoesNotContain(resourceLogs, log => log.Category is { Length: > 0 } category && assertableResourceLogNames.Contains(category) && log.Level >= LogLevel.Error);

		static bool ShouldAssertErrorsForResource(IResource resource)
		{
			return resource
				is
					// Container resources tend to write to stderr for various reasons so only assert projects and executables
					(ProjectResource or ExecutableResource)
					// Node resources tend to have npm modules that write to stderr so ignore them
					and not NodeAppResource
				// Dapr resources write to stderr about deprecated --components-path flag
				&& !resource.Name.EndsWith("-dapr-cli", StringComparison.InvariantCulture);
		}
	}

	/// <summary>
	/// Gets the app host and resource logs from the application.
	/// </summary>
	public static (IReadOnlyList<FakeLogRecord> AppHostLogs, IReadOnlyList<FakeLogRecord> ResourceLogs) GetLogs(this DistributedApplication app)
	{
		ArgumentNullException.ThrowIfNull(app);

		var environment = app.Services.GetRequiredService<IHostEnvironment>();
		var logCollector = app.Services.GetFakeLogCollector();
		var logs = logCollector.GetSnapshot();
		var appHostLogs = logs.Where(l => l.Category?.StartsWith($"{environment.ApplicationName}.Resources", StringComparison.InvariantCulture) == false).ToList();
		var resourceLogs = logs.Where(l => l.Category?.StartsWith($"{environment.ApplicationName}.Resources", StringComparison.InvariantCulture) == true).ToList();

		return (appHostLogs, resourceLogs);
	}

	/// <summary>
	/// Ensures all parameters in the application configuration have values set.
	/// </summary>
	public static TBuilder WithRandomParameterValues<TBuilder>(this TBuilder builder)
		where TBuilder : IDistributedApplicationTestingBuilder
	{
		var parameters = builder.Resources.OfType<ParameterResource>().Where(p => !p.IsConnectionString).ToList();
		foreach (var parameter in parameters)
		{
			builder.Configuration[$"Parameters:{parameter.Name}"] = parameter.Secret
				? PasswordGenerator.Generate(16, true, true, true, false, 1, 1, 1, 0)
				: Convert.ToHexString(RandomNumberGenerator.GetBytes(4));
		}

		return builder;
	}

	/// <summary>
	/// Replaces all named volumes with anonymous volumes so they're isolated across test runs and from the volume the app uses during development.
	/// </summary>
	/// <remarks>
	/// Note that if multiple resources share a volume, the volume will instead be given a random name so that it's still shared across those resources in the test run.
	/// </remarks>
	public static TBuilder WithRandomVolumeNames<TBuilder>(this TBuilder builder)
		where TBuilder : IDistributedApplicationTestingBuilder
	{
		// Named volumes that aren't shared across resources should be replaced with anonymous volumes.
		// Named volumes shared by mulitple resources need to have their name randomized but kept shared across those resources.

		// Find all shared volumes and make a map of their original name to a new randomized name
		var allResourceNamedVolumes = builder.Resources.SelectMany(r => r.Annotations
			.OfType<ContainerMountAnnotation>()
			.Where(m => m.Type == ContainerMountType.Volume && !string.IsNullOrEmpty(m.Source))
			.Select(m => (Resource: r, Volume: m)))
			.ToList();
		var seenVolumes = new HashSet<string>();
		var renamedVolumes = new Dictionary<string, string>();
		foreach (var resourceVolume in allResourceNamedVolumes)
		{
			var name = resourceVolume.Volume.Source!;
			if (!seenVolumes.Add(name) && !renamedVolumes.ContainsKey(name))
			{
				renamedVolumes[name] = $"{name}-{Convert.ToHexString(RandomNumberGenerator.GetBytes(4))}";
			}
		}

		// Replace all named volumes with randomly named or anonymous volumes
		foreach (var resourceVolume in allResourceNamedVolumes)
		{
			var resource = resourceVolume.Resource;
			var volume = resourceVolume.Volume;
			var newName = renamedVolumes.TryGetValue(volume.Source!, out var randomName) ? randomName : null;
			var newMount = new ContainerMountAnnotation(newName, volume.Target, ContainerMountType.Volume, volume.IsReadOnly);
			resource.Annotations.Remove(volume);
			resource.Annotations.Add(newMount);
		}

		return builder;
	}
}
