namespace WebAPI.Filters;

public class ApiKeyAuthenticationFilter(IConfiguration configuration) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var apiKey = configuration["AppApiKey"];
        var xApiKeyHeader = context.HttpContext.Request.Headers["x-api-key"];
        
        Console.WriteLine($"apiKey {apiKey}");
        Console.WriteLine($"xApiKeyHeader {xApiKeyHeader}");
        Console.WriteLine($"apiKey == xApiKeyHeader {apiKey == xApiKeyHeader}");

        if (apiKey != xApiKeyHeader) return Results.Unauthorized();

        return await next(context);
    }
}