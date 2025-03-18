using Carter;

using FluentValidation;
using Scalar.AspNetCore;
using WinHub.ApiService.Database;

var builder = WebApplication.CreateBuilder(args);

// Add Database
builder.AddNpgsqlDbContext<WinHubContext>("winhubdb");

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

builder.Services.AddCarter();

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();

	app.MapOpenApi();
	app.MapScalarApiReference();

	using var scope = app.Services.CreateScope();
	var context = scope.ServiceProvider.GetRequiredService<WinHubContext>();
	await context.Database.EnsureCreatedAsync().ConfigureAwait(true);
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days.
	// You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.MapCarter();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();

await app.RunAsync().ConfigureAwait(true);
