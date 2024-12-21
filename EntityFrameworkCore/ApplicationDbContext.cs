using EntityFrameworkCore.Rules;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine("OnConfiguring");
        optionsBuilder.AddInterceptors(new PreventEfUpdateInterceptor());
        base.OnConfiguring(optionsBuilder);
    }
}