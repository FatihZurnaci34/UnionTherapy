using UnionTherapy.Persistence;
using UnionTherapy.Application;
using UnionTherapy.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UnionTherapy.Infrastructure.Utility.JWT;
using UnionTherapyAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Swagger UI için gerekli servisler
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Controllers
builder.Services.AddControllers();

// Persistence services (PostgreSQL)
builder.Services.AddPersistenceServices(builder.Configuration);

// Application services
builder.Services.AddApplicationServices();

// Infrastructure services (JWT, Security)
builder.Services.AddInfrastructureServices(builder.Configuration);

// JWT Authentication (ASP.NET Core built-in - opsiyonel, custom middleware kullanacağız)
var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
if (jwtOptions == null)
    throw new InvalidOperationException("JWT konfigürasyonu bulunamadı");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS yönlendirmesi sadece production'da
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// ⚠️ ÖNEMLİ: Exception Middleware EN ÜSTTE OLMALI!
app.UseMiddleware<ExceptionMiddleware>();

// Custom Token Check Middleware (API katmanında)
app.UseTokenCheck();

// Built-in Authentication & Authorization (sonra bunlar)
app.UseAuthentication();
app.UseAuthorization();

// Ana sayfa yönlendirmesi sadece development'ta
if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/swagger"))
       .ExcludeFromDescription();
}

// Map controllers
app.MapControllers();

app.Run();
