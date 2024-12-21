using EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ApiHost;

public static class AuthorizationConfiguration
{
    public static void ConfigureAuthorization(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();
        
        builder.Services.AddIdentityApiEndpoints<IdentityUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        // builder.Services.Configure<IdentityOptions>(options =>
        // {
        //     options.SignIn.RequireConfirmedEmail = true;
        // });
        // need to implement email sender -> that needs some sender
        // https://learn.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-9.0&tabs=visual-studio
        // builder.Services.AddTransient<IEmailSender, EmailSender>();
    }
}