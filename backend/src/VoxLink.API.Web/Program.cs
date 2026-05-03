using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VoxLink.API.Application;
using VoxLink.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();

// Add application layer
builder.Services.AddApplication();

// Add infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);

// Add authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

// Add SignalR hub
app.MapHub<VoxLink.API.Infrastructure.Hubs.ChatHub>("/chatHub");

app.Run();
