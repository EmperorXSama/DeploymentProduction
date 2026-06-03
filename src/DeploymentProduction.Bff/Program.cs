var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddHttpClient("ApiClient", client =>
{
    var apiBaseUrl = builder.Configuration["Services:ApiBaseUrl"];

    if (string.IsNullOrWhiteSpace(apiBaseUrl))
    {
        throw new InvalidOperationException("Missing configuration value: Services:ApiBaseUrl");
    }

    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "healthy",
        service = "DeploymentDemo.Bff",
        environment = app.Environment.EnvironmentName,
        checkedAtUtc = DateTimeOffset.UtcNow
    });
});

app.MapGet("/bff/system/info", (IConfiguration configuration, IWebHostEnvironment environment) =>
{
    return Results.Ok(new
    {
        service = "DeploymentDemo.Bff",
        environment = environment.EnvironmentName,
        machineName = Environment.MachineName,
        aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
        apiBaseUrl = configuration["Services:ApiBaseUrl"],
        checkedAtUtc = DateTimeOffset.UtcNow
    });
});

app.MapGet("/bff/api-info", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("ApiClient");

    var response = await client.GetAsync("/api/system/info");

    if (!response.IsSuccessStatusCode)
    {
        return Results.Problem(
            title: "API call failed",
            detail: $"API returned status code {(int)response.StatusCode}",
            statusCode: 502);
    }

    var content = await response.Content.ReadFromJsonAsync<object>();

    return Results.Ok(new
    {
        service = "DeploymentDemo.Bff",
        message = "BFF successfully called API.",
        apiResponse = content,
        checkedAtUtc = DateTimeOffset.UtcNow
    });
});

app.Run();