using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiHost;

public static class DatabaseConfiguration
{
    public static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("AppDb"));
        
    }
}