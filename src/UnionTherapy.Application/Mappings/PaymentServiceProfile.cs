using AutoMapper;
using UnionTherapy.Application.Models.Payment.Request;
using UnionTherapy.Application.Models.Payment.Response;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Mappings;

public class PaymentServiceProfile : Profile
{
    public PaymentServiceProfile()
    {
        CreateMap<Payment, PaymentGetByIdResponse>()
            .ForMember(c => c.UserName, opt => opt.MapFrom(c => $"{c.User.FirstName} {c.User.LastName}"))
            .ReverseMap();
        
        CreateMap<CreatePaymentRequest, Payment>().ReverseMap();
        CreateMap<Payment, CreatePaymentResponse>().ReverseMap();
    }
} 