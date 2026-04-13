using AutoMapper;
using FCG.Application.DTOs;
using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using FCG.Domain.Views;

namespace FCG.Application.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<Email, string>().ConvertUsing(e => e.Value);

        CreateMap<CPF, string>().ConvertUsing(e => e.Code);

        CreateMap<User, UserCreate>().ReverseMap();
        CreateMap<User, AdminCreate>().ReverseMap();

        CreateMap<User, UserView>().ReverseMap();

        CreateMap<User, UserResponse>().ReverseMap();

        CreateMap<UserView, UserResponse>().ReverseMap();
    }
}

