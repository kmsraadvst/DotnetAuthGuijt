namespace WebAPI.Extensions;

public static class ResourceExtension
{
    private static List<Resource> _resources =
    [
        new(Guid.Parse("a26d3001-98e6-492e-ae5b-2931f44b69ff"), "vélo"),
        new(Guid.Parse("9124329a-b60a-4dce-ae99-56a2784409d0"), "radio"),
        new(Guid.Parse("a5c15ed6-2dcd-4fd1-8c4c-c3bae616fb7f"), "casserole"),
        new(Guid.Parse("da1e1ac6-d687-4393-807b-a7e602a2998d"), "chaise"),
    ];
    
    public static WebApplication MapResource(this WebApplication app)
    {
        var resourceGroup = app
            .MapGroup("/resource")
            .RequireAuthorization();

        resourceGroup.MapGet("/",  () => Results.Ok(_resources));
        
        resourceGroup.MapPost("/", ([FromBody] string resourceStr) =>
        {
            var guid = Guid.NewGuid();
            var resource = new Resource(guid, resourceStr);
            
            _resources.Add(resource);
            
            return Results.Created($"/{guid}", resource);
        });

        resourceGroup.MapDelete("/{guidstr}", (string guidstr) =>
        {
            if (Guid.TryParse(guidstr, out Guid guid))
            {
                var itemsRemoved = _resources.RemoveAll(r => r.Guid == guid);
                Console.WriteLine($"resource(s) supprimée(s) : {itemsRemoved}");
            }

            return Results.NoContent();
        });
        
        return app;
    }
}