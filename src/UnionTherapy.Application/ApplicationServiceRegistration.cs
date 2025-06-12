using Microsoft.Extensions.DependencyInjection;
using UnionTherapy.Application.Services.UserService;
using UnionTherapy.Application.Services.SessionService;
using UnionTherapy.Application.Services.PsychologistService;
using UnionTherapy.Application.Services.PaymentService;
using UnionTherapy.Application.Services.ReviewService;
using UnionTherapy.Application.Services.NotificationService;
using UnionTherapy.Application.Services.AuthService;
using UnionTherapy.Application.Services.Localization;
using UnionTherapy.Application.Utilities;

namespace UnionTherapy.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper registration
        services.AddAutoMapper(typeof(ApplicationServiceRegistration));
        
        // Localization Services
        services.AddScoped<ILocalizationService, LocalizationService>();

        // Service registrations
        services.AddScoped<IAuthService , AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IPsychologistService, PsychologistService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
} 