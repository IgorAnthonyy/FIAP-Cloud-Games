using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.ViewModels;
using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Application.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
        }
    }
}
