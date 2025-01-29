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

// Setip Angular Frontend
builder.AddNpmApp("angular", "../WinHub.Web.Angular")
	.WithReference(apiService)
	.WithHttpEndpoint(env: "PORT")
	.WithExternalHttpEndpoints()
	.PublishAsDockerFile();

// Setup Blazor frontend
// builder.AddProject<Projects.WinHub_Web>("webfrontend")
// 	.WithExternalHttpEndpoints()
#pragma warning disable S125 // Sections of code should not be commented out
// 	.WithReference(apiService);


await builder.Build().RunAsync().ConfigureAwait(true);
#pragma warning restore S125 // Sections of code should not be commented out
