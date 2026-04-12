using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.ViewModels;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.IService;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Services
{
    public class UserService : BaseApplicationService, IUserService
    {
        private readonly IValidator<UserDTO> _validator;
        private readonly IEmailService _emailService;
        private readonly IUserDomainService _userDomainService;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, 
            IValidator<UserDTO> validator, 
            IMapper mapper,  
            IEmailService emailService,
            IUserDomainService userDomainService) : base(unitOfWork)
        {
            _validator = validator;
            _mapper = mapper;
            _emailService = emailService;
            _userDomainService = userDomainService;
        }

        public async Task<UserViewModel> CreateUser(UserDTO user)
        {
            var validation = await _validator.ValidateAsync(user);
            if (!validation.IsValid) throw new ApplicationException("Erro na validação");
            var userMapped = _mapper.Map<User>(user);
            var insertedUser = await _userDomainService.CreateUser(userMapped, "PADRAO");
            UserViewModel userView = _mapper.Map<UserViewModel>(insertedUser);

            await _emailService.SendAsync(userView);

            return userView;
        }

       
    }
}
