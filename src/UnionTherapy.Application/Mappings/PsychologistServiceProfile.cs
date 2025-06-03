using AutoMapper;
using UnionTherapy.Application.Models.Psychologist.Request;
using UnionTherapy.Application.Models.Psychologist.Response;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Mappings;

public class PsychologistServiceProfile : Profile
{
    public PsychologistServiceProfile()
    {
        CreateMap<Psychologist, PsychologistGetByIdResponse>()
            .ForMember(c => c.UserName, opt => opt.MapFrom(c => $"{c.User.FirstName} {c.User.LastName}"))
            .ForMember(c => c.UserEmail, opt => opt.MapFrom(c => c.User.Email))
            .ReverseMap();
        
        CreateMap<CreatePsychologistRequest, Psychologist>().ReverseMap();
        CreateMap<Psychologist, CreatePsychologistResponse>().ReverseMap();
    }
} 