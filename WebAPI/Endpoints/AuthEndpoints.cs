using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Endpoints;

public static class AuthEndpoints
{
    public static WebApplication MapAuth(this WebApplication app)
    {
        app.MapGet("/hukar-login", async ([FromQuery] string returnUrl, HttpContext ctx) =>
        {
            Console.WriteLine($"return URL: {returnUrl}");

            var claimOne = new Claim("name", "hukar");
            var claimTwo = new Claim("password", "1234");
            var identity = new ClaimsIdentity([claimOne, claimTwo], CookieAuthenticationDefaults.AuthenticationScheme);

            var user = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
            };

            await ctx.SignInAsync(user, authProperties);
        });

        app.MapGet("/hukar-logout", async (HttpContext ctx) => { await ctx.SignOutAsync(); });

        app.MapPost("/account", (Credential credential, IConfiguration configuration) =>
        {
            // Vérifier les credentials (via une DB par exemple)
            if (credential is { Name: "hukar", Password: "xxx" })
            {
                var secret = configuration["Authentication:SecretForKey"] ?? "";
                Console.WriteLine($"secret: {secret}");

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                Console.WriteLine(securityKey);

                var signingCredentials = new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256
                );
                Console.WriteLine(
                    $"signingCredential Kid:{signingCredentials.Kid} Digest:{signingCredentials.Digest} Algorithm:{signingCredentials.Algorithm}");

                List<Claim> claimsForToken =
                [
                    new("sub", "1234"), // user id
                    new("givenname", "hukar"),
                    new("email", "hukar@red.be"),
                    new(ClaimTypes.Role, "dev"),
                ];

                var securityToken = new JwtSecurityToken(
                    configuration["Authentication:Issuer"],
                    configuration["Authentication:Audience"],
                    claimsForToken,
                    DateTime.Now, // begin validity notBefore
                    DateTime.Now.AddHours(1), // finish validity Expires
                    signingCredentials
                );
                Console.WriteLine(securityToken);
                
                var tokenString = new JwtSecurityTokenHandler()
                    .WriteToken(securityToken);
                Console.WriteLine(tokenString);

                return Ok(tokenString);

            }

            return Unauthorized();
        });

        return app;
    }
}