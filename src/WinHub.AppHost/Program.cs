var builder = DistributedApplication.CreateBuilder(args);



// Setup Database
var dbName = "postgresdb";
var postgres = builder.AddPostgres("postgres")
	.WithEnvironment("POSTGRES_DB", dbName);
var postgresdb = postgres.AddDatabase(dbName);

// Setup backend
var apiService = builder.AddProject<Projects.WinHub_ApiService>("apiservice")
	.WithReference(postgresdb);

// Setup Blazor frontend
builder.AddProject<Projects.WinHub_Web>("webfrontend")
	.WithExternalHttpEndpoints()
	.WithReference(apiService);

await builder.Build().RunAsync().ConfigureAwait(true);
