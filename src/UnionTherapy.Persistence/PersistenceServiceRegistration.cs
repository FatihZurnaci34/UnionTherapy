using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // PostgreSQL bağlantısı
        services.AddDbContext<BaseDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("UnionTherapyDb")));

        return services;
    }
} 