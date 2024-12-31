namespace ApiHost;

public static class CorsConfiguration
{
    public const string CorsAllowAll = "AllowAll";
    public const string CorsAllowSpecific = "AllowSpecificOrigin";
    
    public static void ConfigureCors(WebApplicationBuilder builder)
    {
        var corsOptions = builder.Configuration.GetSection("Cors").Get<CorsOptions>();
        
        var allowedOrigins = Environment.GetEnvironmentVariable("ASPNETCORE_CORS_AllowedOrigins") ??
                             corsOptions?.AllowedOrigins;

        var allowedMethods = Environment.GetEnvironmentVariable("ASPNETCORE_CORS_AllowedMethods") ??
                             corsOptions?.AllowedMethods;
        
        var allowedHeaders = Environment.GetEnvironmentVariable("ASPNETCORE_CORS_AllowedHeaders") ??
                             corsOptions?.AllowedHeaders;
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CorsAllowSpecific,
                policy =>
                {
                    policy.WithOrigins(allowedOrigins?.Split(", ") ?? ["https://localhost:5173"])
                        .WithMethods(allowedMethods?.Split(", ") ?? ["GET", "POST", "PUT", "DELETE"])
                        .WithHeaders(allowedHeaders?.Split(", ") ?? ["Content-Type", "Authorization"])
                        .AllowCredentials();
                });
            
            options.AddPolicy(name: CorsAllowAll,
                policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }
}