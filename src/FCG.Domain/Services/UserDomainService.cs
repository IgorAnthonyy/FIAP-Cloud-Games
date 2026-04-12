using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Domain.Services
{
    public class UserDomainService : BaseDomainService, IUserDomainService
    {

        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;
        public UserDomainService(IUnitOfWork unitOfWork, IUserRepository userRepository, IPasswordHashService passwordHashService) : base(unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
        }

        
        

        public async Task<User> CreateUser(User user, string role)
        {

            var userSearch = await _userRepository.GetByEmail(user.Email);

            if (userSearch != null) throw new ApplicationException("Usuário encontrado");

            user.AddRole(new Role
            {
                Name = role,
            });
            user.Password = _passwordHashService.GenerateHash(user.Password);
            user.Situation = true;
            User insertedUser = await _userRepository.Insert(user);

            await UnitOfWork.CommitAsync();

            return insertedUser;


        }
    }
}
