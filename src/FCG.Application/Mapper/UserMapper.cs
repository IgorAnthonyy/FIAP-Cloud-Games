using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.ViewModels;
using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Application.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<Email, string>().ConvertUsing(e => e.Value);
            CreateMap<CPF, string>().ConvertUsing(e => e.Code);
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
        }
    }
}
