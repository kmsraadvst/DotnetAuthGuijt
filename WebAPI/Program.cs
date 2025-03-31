var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    // .AddCookie(options =>
    // {
    //     options.LoginPath = "/hukar-login";
    //     options.LogoutPath = "/hukar-logout";
    //     options.Cookie.HttpOnly = true;
    //     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    //     options.SlidingExpiration = true;
    //     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    //     options.Cookie.SameSite = SameSiteMode.Strict;
    //     options.Cookie.Domain = "localhost";
    // });

builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapResource();
app.MapAuth();

app.Run();

