var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.WinHub_ApiService>("apiservice");

builder.AddProject<Projects.WinHub_Web>("webfrontend")
	.WithExternalHttpEndpoints()
	.WithReference(apiService);

await builder.Build().RunAsync().ConfigureAwait(true);
