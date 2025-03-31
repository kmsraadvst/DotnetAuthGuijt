namespace BlazorWebServer.Repositories;

public class ProductRepository(IHttpClientFactory factory)
{
    public async Task CreateAsync(string resource)
    {
        var client = factory.CreateClient("api-hukar");

        await client.PostAsJsonAsync("/resource", resource);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var client = factory.CreateClient("api-hukar");

        return await client.GetFromJsonAsync<IEnumerable<Product>>("/resource") ?? [];
    }

    public async Task DeleteAsync(Guid id)
    {
        var client = factory.CreateClient("api-hukar");

        await client.DeleteAsync($"/resource/{id}");
    }
}