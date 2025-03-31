var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication()
    .AddCookie("HukarSchemeCookie");
builder.Services.AddAuthorization();

var apiKey = builder.Configuration["ApiKey"];

Console.WriteLine($"api key {apiKey}");

builder.Services.AddHttpClient("api-hukar", cfg =>
{
    cfg.BaseAddress = new Uri("http://localhost:5005");
    cfg.DefaultRequestHeaders.Add("x-api-key", apiKey);
});

builder.Services.AddMudServices();

builder.Services.AddSingleton<ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();