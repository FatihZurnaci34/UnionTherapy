using AutoMapper;
using UnionTherapy.Application.Models.Review.Request;
using UnionTherapy.Application.Models.Review.Response;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Mappings;

public class ReviewServiceProfile : Profile
{
    public ReviewServiceProfile()
    {
        CreateMap<Review, ReviewGetByIdResponse>()
            .ForMember(c => c.SessionName, opt => opt.MapFrom(c => c.Session.Name))
            .ForMember(c => c.UserName, opt => opt.MapFrom(c => $"{c.User.FirstName} {c.User.LastName}"))
            .ReverseMap();
        
        CreateMap<CreateReviewRequest, Review>().ReverseMap();
    }
} 