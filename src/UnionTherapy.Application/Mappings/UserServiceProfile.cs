using AutoMapper;
using UnionTherapy.Application.Models.User.Request;
using UnionTherapy.Application.Models.User.Response;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Mappings;

public class UserServiceProfile : Profile
{
    public UserServiceProfile()
    {
        CreateMap<User, UserGetByIdResponse>().ReverseMap();
        CreateMap<CreateUserRequest, User>().ReverseMap();
        CreateMap<UpdateUserRequest, User>().ReverseMap();
    }
} 