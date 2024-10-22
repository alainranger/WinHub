using System.Collections.ObjectModel;
using System.Text.Json;

using WinHub.ApiService.IntegrationTests.Infrastructure;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace WinHub.ApiService.IntegrationTests.Tests;

public class AppHostTests(ITestOutputHelper testOutput)
{
	[Theory]
	[MemberData(nameof(TestEndpoints))]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>")]
	public async Task TestEndpointsReturnOk(TestEndpoints testEndpoints)
	{
		ArgumentNullException.ThrowIfNull(testEndpoints);

		var appHostName = testEndpoints.AppHost!;
		var resourceEndpoints = testEndpoints.ResourceEndpoints!;

		var appHostPath = $"{appHostName}.dll";
		var appHost = await DistributedApplicationTestFactory.CreateAsync(appHostPath, testOutput);

		await using var app = await appHost.BuildAsync();

		await app.StartAsync();
		await app.WaitForResources().WaitAsync(TimeSpan.FromSeconds(30));

		if (testEndpoints.WaitForResources?.Count > 0)
		{
			// Wait until each resource transitions to the required state
			var timeout = TimeSpan.FromMinutes(5);
			foreach (var (ResourceName, TargetState) in testEndpoints.WaitForResources)
			{
				await app.WaitForResource(ResourceName, TargetState).WaitAsync(timeout);
			}
		}

		foreach (var resource in resourceEndpoints.Keys)
		{
			var endpoints = resourceEndpoints[resource];

			if (endpoints.Count == 0)
			{
				// No test endpoints so ignore this resource
				continue;
			}

			HttpResponseMessage? response = null;

			using var client = app.CreateHttpClient(resource, null, clientBuilder =>
			{
				clientBuilder
					.ConfigureHttpClient(client => client.Timeout = Timeout.InfiniteTimeSpan)
					.AddStandardResilienceHandler(resilience =>
					{
						resilience.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(120);
						resilience.AttemptTimeout.Timeout = TimeSpan.FromSeconds(60);
						resilience.Retry.MaxRetryAttempts = 30;
						resilience.CircuitBreaker.SamplingDuration = resilience.AttemptTimeout.Timeout * 2;
					});
			});

			foreach (var path in endpoints)
			{
				testOutput.WriteLine($"Calling endpoint '{client.BaseAddress}{path.TrimStart('/')} for resource '{resource}' in app '{Path.GetFileNameWithoutExtension(appHostPath)}'");
				try
				{
					response = await client.GetAsync(new Uri(path, UriKind.Relative)).ConfigureAwait(true);
				}
				catch (Exception e)
				{
					throw new XunitException($"Failed calling endpoint '{client.BaseAddress}{path.TrimStart('/')} for resource '{resource}' in app '{Path.GetFileNameWithoutExtension(appHostPath)}'", e);
				}

				Assert.True(HttpStatusCode.OK == response.StatusCode, $"Endpoint '{client.BaseAddress}{path.TrimStart('/')}' for resource '{resource}' in app '{Path.GetFileNameWithoutExtension(appHostPath)}' returned status code {response.StatusCode}");

				var content = await response.Content.ReadAsStringAsync();
				Assert.NotEmpty(content);
			}
		}

		app.EnsureNoErrorsLogged();

		await app.StopAsync();
	}

	public static TheoryData<TestEndpoints> TestEndpoints() =>
		new() {
			new TestEndpoints("WinHub.AppHost", new() {
				{ "apiservice", ["/api/contests", "/health", "/alive"] }
			})
		};
}

public class TestEndpoints : IXunitSerializable
{
	// Required for deserialization
	public TestEndpoints() { }

	public TestEndpoints(string appHost, Dictionary<string, List<string>> resourceEndpoints)
	{
		AppHost = appHost;
		ResourceEndpoints = resourceEndpoints;
	}

	public string? AppHost { get; set; }

	public ReadOnlyCollection<ResourceWait>? WaitForResources { get; set; }

	public Dictionary<string, List<string>>? ResourceEndpoints { get; private set; }

	public void Deserialize(IXunitSerializationInfo info)
	{
		ArgumentNullException.ThrowIfNull(info);

		AppHost = info.GetValue<string>(nameof(AppHost));
		WaitForResources = JsonSerializer.Deserialize<ReadOnlyCollection<ResourceWait>>(info.GetValue<string>(nameof(WaitForResources)));
		ResourceEndpoints = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(info.GetValue<string>(nameof(ResourceEndpoints)));
	}

	public void Serialize(IXunitSerializationInfo info)
	{
		ArgumentNullException.ThrowIfNull(info);

		info.AddValue(nameof(AppHost), AppHost);
		info.AddValue(nameof(WaitForResources), JsonSerializer.Serialize(WaitForResources));
		info.AddValue(nameof(ResourceEndpoints), JsonSerializer.Serialize(ResourceEndpoints));
	}

	public override string? ToString() => $"{AppHost} ({ResourceEndpoints?.Count ?? 0} resources)";


}
public class ResourceWait(string resourceName, string targetState)
{
	public string ResourceName { get; } = resourceName;

	public string TargetState { get; } = targetState;

	public void Deconstruct(out string resourceName, out string targetState)
	{
		resourceName = ResourceName;
		targetState = TargetState;
	}
}
