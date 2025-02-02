var builder = DistributedApplication.CreateBuilder(args);

// Setup Database
var dbName = "winhubdb";
var postgres = builder.AddPostgres("postgres")
	.WithPgAdmin();

var database = postgres.AddDatabase(dbName);

// Setup backend
var apiService = builder.AddProject<Projects.WinHub_ApiService>("apiservice")
	.WithReference(database)
	.WaitFor(database);

// Setup Blazor frontend
builder.AddProject<Projects.WinHub_Blazor>("webfrontend")
	.WithExternalHttpEndpoints()
	.WithReference(apiService);

await builder.Build().RunAsync().ConfigureAwait(true);
