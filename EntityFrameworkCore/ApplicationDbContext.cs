using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (Environment.GetEnvironmentVariable("RUNNING_MIGRATIONS") != "true")
        {
            throw new InvalidOperationException("Direct database updates using 'dotnet ef database update' are not allowed.");
        }
        
        Console.WriteLine($"{GetType().Name} OnConfiguring called");
        base.OnConfiguring(optionsBuilder);
    }
}