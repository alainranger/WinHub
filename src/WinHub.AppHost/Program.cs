var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql");
var sqldb = sql.AddDatabase("sqldb");

var apiService = builder.AddProject<Projects.WinHub_ApiService>("apiservice").
	WithReference(sqldb);

builder.AddProject<Projects.WinHub_Web>("webfrontend")
	.WithExternalHttpEndpoints()
	.WithReference(apiService);

await builder.Build().RunAsync().ConfigureAwait(true);
