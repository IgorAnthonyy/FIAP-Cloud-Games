using AutoMapper;
using CommonTestUtilities.Entities;
using FCG.Application.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Respositories;
using FCG.Tests.Fixture;
using Moq;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests
{
    [Collection(nameof(UserFixtureCollection))]
    [Binding]
    public class AuthStepDefinition
    {
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IUserLogged> _userLogged = new();
        public UserBuilder _userFixture;
        private User userAdmin = null!;
        private readonly AdminContext _adminContext;
        public AuthStepDefinition(UserBuilder userFixture, AdminContext adminContext)
        {
            _userFixture = userFixture;
            _adminContext = adminContext;
        }
        [Given("que sou admin do sistema")]
        public void GivenQueSouAdminDoSistema()
        {
            userAdmin = _userFixture.GenerateUserWithRoleAdmin();
            _userLogged.Setup(x => x.IsAdmin).Returns(true);
            _userLogged.Setup(x => x.UserId).Returns(userAdmin.Id);

            _userRepo.Setup(x => x.GetByEmail(It.IsAny<string>()))
                 .ReturnsAsync(userAdmin);
            _adminContext.UserRepo = _userRepo;
            _adminContext.UserAdmin = userAdmin;
            _adminContext.UserLogged = _userLogged;
        }

        [Given("que não sou admin do sistema")]
        public void GivenQueNaoSouAdminDoSistema()
        {
            userAdmin = _userFixture.GenerateUserWithRolesDefault();
            _userLogged.Setup(x => x.IsAdmin).Returns(false);
            _userLogged.Setup(x => x.UserId).Returns(userAdmin.Id);

            _userRepo.Setup(x => x.GetByEmail(It.IsAny<string>()))
                 .ReturnsAsync(userAdmin);
            _adminContext.UserRepo = _userRepo;
            _adminContext.UserAdmin = userAdmin;
            _adminContext.UserLogged = _userLogged;
        }
    }
}
