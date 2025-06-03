using AutoMapper;
using UnionTherapy.Application.Models.Session.Request;
using UnionTherapy.Application.Models.Session.Response;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Mappings;

public class SessionServiceProfile : Profile
{
    public SessionServiceProfile()
    {
        CreateMap<Session, SessionGetByIdResponse>()
            .ForMember(c => c.PsychologistName, opt => opt.MapFrom(c => 
                c.Psychologist != null ? $"{c.Psychologist.User.FirstName} {c.Psychologist.User.LastName}" : string.Empty))
            .ForMember(c => c.CurrentParticipants, opt => opt.MapFrom(c => c.Participants.Count))
            .ReverseMap();
        
        CreateMap<CreateSessionRequest, Session>().ReverseMap();
        CreateMap<UpdateSessionRequest, Session>().ReverseMap();
    }
} 