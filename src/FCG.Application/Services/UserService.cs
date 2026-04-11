using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.ViewModels;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Services
{
    public class UserService : BaseApplicationService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IValidator<UserDTO> _validator;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IPasswordHashService passwordHashService, IUserRepository userRepository, IValidator<UserDTO> validator, IMapper mapper, IRoleRepository roleRepository) : base(unitOfWork)
        {
            _userRepository = userRepository;
            _validator = validator;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task<UserViewModel> CriarUsuario(UserDTO user)
        {
            var userView = await CriarUsuario(user, "PADRAO");
            //Envia Email aqui de boas vindas
            return userView;
        }

        private async Task<UserViewModel> CriarUsuario(UserDTO user, string role)
        {
            var validation = await _validator.ValidateAsync(user);
            if (!validation.IsValid) throw new ApplicationException("Erro na validação");
            var userSearch = await _userRepository.GetByEmail(user.Email);

            if (userSearch != null) throw new ApplicationException("Usuário encontrado");

            User userMapper = _mapper.Map<User>(user);

            userMapper.AdicionarRole(new Role
            {
                Name = role,
            });
            userMapper.Password = _passwordHashService.GenerateHash(userMapper.Password);
            userMapper.Situation = true;
            User insertedUser = await _userRepository.Insert(userMapper);

            await UnitOfWork.CommitAsync();

            UserViewModel userView = _mapper.Map<UserViewModel>(insertedUser);

            return userView;


        }
    }
}
