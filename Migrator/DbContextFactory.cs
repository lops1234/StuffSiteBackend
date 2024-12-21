﻿using System.Reflection;
using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Migrator;

public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        Console.WriteLine(configuration.GetConnectionString("DefaultConnection"));
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            a => a.MigrationsAssembly(assemblyName));

        return new ApplicationDbContext(optionsBuilder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        var builder = (new ConfigurationBuilder())
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

        return builder.Build();
    }
}