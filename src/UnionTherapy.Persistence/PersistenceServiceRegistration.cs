using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnionTherapy.Application.Repository;
using UnionTherapy.Persistence.Context;
using UnionTherapy.Persistence.Repository;

namespace UnionTherapy.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // PostgreSQL bağlantısı
        services.AddDbContext<BaseDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("UnionTherapyDb")));

        // Repository kayıtları
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPsychologistRepository, PsychologistRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IPsychologistReviewRepository, PsychologistReviewRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<IUserContractRepository, UserContractRepository>();
        services.AddScoped<ISessionParticipationRepository, SessionParticipationRepository>();
        services.AddScoped<IPsychologistDocumentRepository, PsychologistDocumentRepository>();
        services.AddScoped<IPsychologistSpecializationRepository, PsychologistSpecializationRepository>();

        return services;
    }
} 