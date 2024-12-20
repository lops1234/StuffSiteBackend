using System.Security.Cryptography.X509Certificates;

namespace ApiHost.Kestrel;

public static class KestrelConfiguration
{
    public static void ConfigureKestrel(WebApplicationBuilder builder)
    {
        CertificateConfiguration(builder);
        PortConfiguration(builder);
    }

    private static void CertificateConfiguration(WebApplicationBuilder builder)
    {
        var certPath = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path") ??
                       builder.Configuration.GetSection("Certificate").GetValue<string>("Path");
        
        var certKey = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password") ??
                      builder.Configuration.GetSection("Certificate").GetValue<string>("Password");
        
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.ServerCertificate = X509CertificateLoader.LoadPkcs12FromFile(certPath!, certKey);
            });
        });
    }

    private static void PortConfiguration(WebApplicationBuilder builder)
    {
        if (HasAccessToLaunchSettings()) return;

        // Read ports from environment variables
        var httpPort = Environment.GetEnvironmentVariable("HTTP_PORT") ?? "5092";
        var httpsPort = Environment.GetEnvironmentVariable("HTTPS_PORT") ?? "7039";

        // Configure Kestrel to use the specified ports
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(int.Parse(httpPort),
                listenOptions =>
                {
                    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
                });
            options.ListenAnyIP(int.Parse(httpsPort), listenOptions => { listenOptions.UseHttps(); });
        });
    }

    private static bool HasAccessToLaunchSettings()
    {
        var launchSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json");
        return File.Exists(launchSettingsPath);
    }
}