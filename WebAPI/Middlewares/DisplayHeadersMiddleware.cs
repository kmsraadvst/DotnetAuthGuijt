namespace WebAPI.Middlewares;

public class DisplayHeadersMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        foreach (var (key, value) in context.Request.Headers)
        {
            Console.WriteLine($"{key}: {value}");
        }
        
        await next(context);

    }
}