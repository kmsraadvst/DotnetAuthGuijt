namespace WebAPI.Endpoints;

public static class ProductEndpoints
{
    private static readonly List<Product> Resources =
    [
        new(Guid.Parse("a26d3001-98e6-492e-ae5b-2931f44b69ff"), "vélo", new DateTime(2025, 2, 1)),
        new(Guid.Parse("9124329a-b60a-4dce-ae99-56a2784409d0"), "radio", new DateTime(2025, 2, 1)),
        new(Guid.Parse("a5c15ed6-2dcd-4fd1-8c4c-c3bae616fb7f"), "casserole", new DateTime(2025, 2, 1)),
        new(Guid.Parse("da1e1ac6-d687-4393-807b-a7e602a2998d"), "chaise", new DateTime(2025, 2, 1)),
    ];
    
    public static WebApplication MapResource(this WebApplication app)
    {
        var resourceGroup = app
            .MapGroup("/resource")
            .RequireAuthorization();

        resourceGroup.MapGet("/",  () => Results.Ok(Resources));
        
        resourceGroup.MapPost("/", ([FromBody] string resourceStr) =>
        {
            Console.WriteLine($"reçu resourceStr: {resourceStr}");
            
            var guid = Guid.NewGuid();
            var resource = new Product(guid, resourceStr, DateTime.Now);
            
            Resources.Add(resource);
            Console.WriteLine($"resource ajoutée: {resource}");

            return Results.Created($"/{guid}", resource);
        })
        .AddEndpointFilter<ApiKeyAuthenticationFilter>();

        resourceGroup.MapDelete("/{guidstr}", (string guidstr) =>
        {
            if (!Guid.TryParse(guidstr, out Guid guid)) return Results.NoContent();
            
            var itemsRemoved = Resources.RemoveAll(r => r.Guid == guid);
            Console.WriteLine($"resource(s) supprimée(s): {itemsRemoved}");

            return Results.NoContent();
        });
        
        return app;
    }
}