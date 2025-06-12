using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UnionTherapy.Application.Models.Auth.Request;
using UnionTherapy.Application.Models.Auth.Response;

using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Mappings;

public class AuthServiceProfile : Profile
{
    public AuthServiceProfile()
    {
        CreateMap<LoginRequest, User>().ReverseMap();
        
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Şifre ayrı işlenecek
            
        CreateMap<User, LoginResponse>()
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.TokenExpiration, opt => opt.Ignore());
    }
}
