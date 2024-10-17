var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("postgresdb");

var apiService = builder.AddProject<Projects.WinHub_ApiService>("apiservice").
	WithReference(postgresdb);

builder.AddProject<Projects.WinHub_Web>("webfrontend")
	.WithExternalHttpEndpoints()
	.WithReference(apiService);

await builder.Build().RunAsync().ConfigureAwait(true);
