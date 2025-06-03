using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnionTherapy.Infrastructure.Utility.JWT;
using UnionTherapy.Application.Interfaces;

namespace UnionTherapy.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // JWT Options konfigürasyonu
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            
            // JWT Token Generator servisini kaydet
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            
            return services;
        }
    }
} 