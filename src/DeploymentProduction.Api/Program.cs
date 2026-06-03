var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// Configure the HTTP request pipeline.

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "healthy",
        service = "DeploymentDemo.Api",
        environment = app.Environment.EnvironmentName,
        checkedAtUtc = DateTimeOffset.UtcNow
    });
});

app.MapGet("/api/system/info", (IConfiguration configuration, IWebHostEnvironment environment) =>
{
    return Results.Ok(new
    {
        service = "DeploymentDemo.Api",
        environment = environment.EnvironmentName,
        machineName = Environment.MachineName,
        aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
        appVersion = configuration["AppVersion"] ?? "not-set",
        checkedAtUtc = DateTimeOffset.UtcNow
    });
});
app.Run();

