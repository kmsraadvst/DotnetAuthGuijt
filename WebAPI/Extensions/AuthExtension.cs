using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace WebAPI.Extensions;

public static class AuthExtension
{
    public static WebApplication MapAuth(this WebApplication app)
    {
        app.MapGet("/hukar-login", async ([FromQuery] string returnUrl, HttpContext ctx) =>
        {
            Console.WriteLine($"return URL: {returnUrl}");

            var claimOne = new Claim("name", "hukar");
            var claimTwo = new Claim("password", "1234");
            var identity = new ClaimsIdentity([ claimOne, claimTwo ], CookieAuthenticationDefaults.AuthenticationScheme);

            var user = new ClaimsPrincipal(identity);
            
            await ctx.SignInAsync(user);
        });

        app.MapGet("/hukar-logout", async (HttpContext ctx) =>
        {
            await ctx.SignOutAsync();
        });

        return app;
    }
}