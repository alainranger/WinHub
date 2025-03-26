using WinHub.Blazor.Components;
using WinHub.Blazor.Mock;
using WinHub.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddOutputCache();

if (builder.Environment.IsDevelopment())
{
	// Use the mock service in development mode.
	builder.Services.AddSingleton<IContestService, MockContestService>();
}
else
{
	builder.Services.AddHttpClient<IContestService, ContestService>(client =>
	{
		// This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
		// Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
		client.BaseAddress = new("https+http://apiservice");
	});
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

await app.RunAsync().ConfigureAwait(true);
