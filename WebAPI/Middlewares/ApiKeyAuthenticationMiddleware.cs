namespace WebAPI.Middlewares;

public class ApiKeyAuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        var apiKey = configuration["AppApiKey"];
        var xApiKeyHeader = context.Request.Headers["x-api-key"];

        Console.WriteLine($"apiKey {apiKey}");
        Console.WriteLine($"xApiKeyHeader {xApiKeyHeader}");
        Console.WriteLine($"apiKey == xApiKeyHeader {apiKey == xApiKeyHeader}");

        if (apiKey != xApiKeyHeader)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await next(context);
    }
}