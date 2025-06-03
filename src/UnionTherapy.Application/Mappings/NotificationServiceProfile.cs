using AutoMapper;
using UnionTherapy.Application.Models.Notification.Request;
using UnionTherapy.Application.Models.Notification.Response;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Mappings;

public class NotificationServiceProfile : Profile
{
    public NotificationServiceProfile()
    {
        CreateMap<Notification, NotificationGetByIdResponse>()
            .ForMember(c => c.UserName, opt => opt.MapFrom(c => $"{c.User.FirstName} {c.User.LastName}"))
            .ReverseMap();
        
        CreateMap<CreateNotificationRequest, Notification>().ReverseMap();
        CreateMap<Notification, CreateNotificationResponse>().ReverseMap();
    }
} 