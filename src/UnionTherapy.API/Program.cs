using UnionTherapy.Persistence;
using UnionTherapy.Application;
using UnionTherapy.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UnionTherapy.Infrastructure.Utility.JWT;
using UnionTherapyAPI.Middlewares;
using UnionTherapy.Application.Utilities;
using UnionTherapy.Application.Services.Localization;
using UnionTherapy.Application.Exceptions;
using UnionTherapy.Application.Constants;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Swagger UI için gerekli servisler
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add Controllers
builder.Services.AddControllers();

// Add HttpContextAccessor for localization
builder.Services.AddHttpContextAccessor();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // Development'ta daha esnek
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            // Production'da güvenli
            policy.WithOrigins("https://yourdomain.com", "https://www.yourdomain.com")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

// Persistence services (PostgreSQL)
builder.Services.AddPersistenceServices(builder.Configuration);

// Application services
builder.Services.AddApplicationServices();

// Infrastructure services (JWT, Security)
builder.Services.AddInfrastructureServices(builder.Configuration);

// JWT Authentication (ASP.NET Core built-in - opsiyonel, custom middleware kullanacağız)
var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
if (jwtOptions == null)
    throw new LocalizedBusinessException(ResponseMessages.JwtConfigurationNotFound);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Rate Limiting Configuration
builder.Services.AddRateLimiter(options =>
{
    // Auth endpoints için daha sıkı limit
    options.AddFixedWindowLimiter("AuthPolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 5; // Dakikada 5 istek
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    // Genel API endpoints için
    options.AddFixedWindowLimiter("GeneralPolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100; // Dakikada 100 istek
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 10;
    });

    // Global fallback
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User?.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 200,
                Window = TimeSpan.FromMinutes(1)
            }));

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.", cancellationToken: token);
    };
});

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

// CORS - Rate limiting'den önce
app.UseCors();

// Rate Limiting
app.UseRateLimiter();

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
