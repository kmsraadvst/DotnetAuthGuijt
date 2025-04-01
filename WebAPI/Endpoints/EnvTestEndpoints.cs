
namespace WebAPI.Endpoints;

public static class EnvTestEndpoints
{
    public static WebApplication MapEnvTest(this WebApplication app)
    {
        app.MapGet("/env-test", (IConfiguration cfg) =>
        {
            var env = cfg["TestEnv"];

            Console.WriteLine($"environment: {env}");

            return Ok(new { Env = env });
        });



        return app;
    }
}