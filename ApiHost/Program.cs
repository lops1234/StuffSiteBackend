using ApiHost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
KestrelConfiguration.ConfigureKestrel(builder);
LoggingConfiguration.ConfigureLogging(builder);
DatabaseConfiguration.ConfigureDatabase(builder);
AuthorizationConfiguration.ConfigureAuthorization(builder);
CorsConfiguration.ConfigureCors(builder);

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationFailureHandler>();

var app = builder.Build();
ConfigureExceptionPage();
ConfigureHttps();
ConfigureApiDocs();

ConfigureCors();
ConfigureAuthorization();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (ILogger<Program> logger) =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .RequireAuthorization();

app.Run();
return;

void ConfigureCors()
{
    app.UseCors(CorsConfiguration.CorsAllowSpecific);
     // app.UseCors(CorsConfiguration.CorsAllowAll);
}

void ConfigureExceptionPage()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    // app.UseExceptionHandler("/error");
    // app.UseStatusCodePagesWithReExecute("/error/{0}");
}

void ConfigureApiDocs()
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }
}

void ConfigureHttps()
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseHttpsRedirection();
}

void ConfigureAuthorization()
{
    app.MapCustomIdentityApi<IdentityUser>();

    // app.UseAuthentication();
    // app.UseAuthorization();
    // app.MapScalarApiReference().RequireAuthorization();
}



record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}