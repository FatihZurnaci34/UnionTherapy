using AutoMapper;
using UnionTherapy.Application.Models.Contract.Request;
using UnionTherapy.Application.Models.Contract.Response;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Mappings;

public class ContractServiceProfile : Profile
{
    public ContractServiceProfile()
    {
        CreateMap<Contract, ContractGetByIdResponse>()
            .ForMember(c => c.PsychologistName, opt => opt.MapFrom(c => c.Psychologist != null ? $"{c.Psychologist.User.FirstName} {c.Psychologist.User.LastName}" : string.Empty))
            .ReverseMap();
        
        CreateMap<CreateContractRequest, Contract>().ReverseMap();
        CreateMap<Contract, CreateContractResponse>().ReverseMap();
    }
} 