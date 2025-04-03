using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Endpoints;
using WebAPI.Middlewares;

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

    var secretBytes = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:secretForKey"] ?? "");

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Authentication:Issuer"],
                ValidAudience = builder.Configuration["Authentication:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
            };
        });

builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<DisplayHeadersMiddleware>();
// app.UseMiddleware<ApiKeyAuthenticationMiddleware>();

app.UseHttpsRedirection();

app.MapEnvTest();
app.MapResource();
app.MapAuth();

app.Run();

