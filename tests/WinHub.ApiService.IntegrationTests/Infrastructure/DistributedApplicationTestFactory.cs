// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace WinHub.ApiService.IntegrationTests.Infrastructure;

internal static class DistributedApplicationTestFactory
{
	/// <summary>
	/// Creates an <see cref="IDistributedApplicationTestingBuilder"/> for the specified app host assembly.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3885:\"Assembly.Load\" should be used", Justification = "<Pending>")]
	public static async Task<IDistributedApplicationTestingBuilder> CreateAsync(string appHostAssemblyPath, ITestOutputHelper? testOutput)
	{
		_ = Path.GetFileNameWithoutExtension(appHostAssemblyPath) ?? throw new InvalidOperationException("AppHost assembly was not found.");

		var appHostAssembly = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, appHostAssemblyPath));

		var appHostType = Array.Find(appHostAssembly.GetTypes(), t => t.Name.EndsWith("_AppHost", StringComparison.InvariantCulture))
			?? throw new InvalidOperationException("Generated AppHost type not found.");

		var builder = await DistributedApplicationTestingBuilder.CreateAsync(appHostType).ConfigureAwait(true);

		builder.WithRandomParameterValues();
		builder.WithRandomVolumeNames();

		builder.Services.AddLogging(logging =>
		{
			logging.ClearProviders();
			logging.AddSimpleConsole();
			logging.AddFakeLogging();
			if (testOutput is not null)
				logging.AddXUnit(testOutput);
			logging.SetMinimumLevel(LogLevel.Trace);
			logging.AddFilter("Aspire", LogLevel.Trace);
			logging.AddFilter(builder.Environment.ApplicationName, LogLevel.Trace);
		});

		return builder;
	}
}
