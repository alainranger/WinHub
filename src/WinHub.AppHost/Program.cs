using WinHub.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

// Setup Database
var dbName = "winhubdb";
var postgres = builder.AddPostgres("postgres")
	.WithPgAdmin();

var database = postgres.AddDatabase(dbName);

// Setup backend
var apiService = builder.AddProject<Projects.WinHub_ApiService>("apiservice")
	.WithScalar()
	.WithReference(database)
	.WaitFor(database);

// Setup Blazor frontend
builder.AddProject<Projects.WinHub_Blazor>("frontend-blazor")
	.WithExternalHttpEndpoints()
	.WithReference(apiService)
	.WaitFor(apiService);

/*

// Setup React frontend
builder.AddNpmApp("frontend-react", "../frontend/WinHub.React")
	.WithReference(apiService)
	.WaitFor(apiService)ª
	.WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
	.WithHttpEndpoint(env: "PORT")
	.WithExternalHttpEndpoints()
	.PublishAsDockerFile();

// Setup Angular frontend
builder.AddNpmApp("frontend-angular", "../frontend/WinHub.Angular")
	.WithReference(apiService)
	.WaitFor(apiService)
	.WithHttpEndpoint(env: "PORT")
	.WithExternalHttpEndpoints()
	.PublishAsDockerFile();

// Setup VueJS frontend
builder.AddNpmApp("frontend-vue", "../frontend/WinHub.VueJS")
	.WithReference(apiService)
	.WaitFor(apiService)
	.WithHttpEndpoint(env: "PORT")
	.WithExternalHttpEndpoints()
	.PublishAsDockerFile();

*/

await builder.Build().RunAsync().ConfigureAwait(true);
