using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Migrator;

Environment.SetEnvironmentVariable("RUNNING_MIGRATIONS", "true");
var host = CreateHostBuilder(args).Build();
ApplyMigrations(host);
return;

static void ApplyMigrations(IHost host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    
    var pendingMigrations = context.Database.GetPendingMigrations().ToList();
    if (pendingMigrations.Any())
    {
        Console.WriteLine("Starting migrations...");

        foreach (var migration in pendingMigrations)
        {
            Console.WriteLine($"Applying migration: {migration}");
            context.Database.Migrate(migration);
        }

        Console.WriteLine("All migrations applied successfully.");
    }
    else
    {
        Console.WriteLine("No pending migrations.");
    }
    
}

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            var dbContextFactory = new DbContextFactory();
            var context = dbContextFactory.CreateDbContext(args);

            services.AddSingleton(context);
        });

