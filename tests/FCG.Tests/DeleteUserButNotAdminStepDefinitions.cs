using AutoMapper;
using CommonTestUtilities.Entities;
using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.Respositories;
using FCG.Domain.Services;
using FCG.Tests.Fixture;
using Moq;
using Reqnroll;
using System;

namespace FCG.Tests
{
    [Collection(nameof(UserFixtureCollection))]
    [Binding]
    public class DeleteUserButNotAdminStepDefinitions
    {
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IEmailService> _email = new();
        private readonly Mock<IPasswordService> _password = new();
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IUserLogged> _userLogged = new();
        public UserBuilder _userFixture;
        private UserService _service;
        private UnauthorizedAccessException _unauthorizedException;
        private User userAdmin;
        private User userRemove;

        public DeleteUserButNotAdminStepDefinitions(UserBuilder userFixture, AdminContext context)
        {
            _userFixture = userFixture;
            _userLogged = context.UserLogged;
            _userRepo = context.UserRepo;
            userAdmin = context.UserAdmin;
        }


        

        [Given("passo um usuário para deletar")]
        public void GivenPassoUmUsuarioParaDeletar()
        {
            userRemove = _userFixture.GenerateUserWithRoleEmpty();
            _userRepo.Setup(x => x.GetById(userRemove.Id))
                .ReturnsAsync(userRemove);

            _userRepo.Setup(x => x.Delete(It.IsAny<User>()))
                     .Returns(userRemove);

            var domain = new UserDomainService(_userRepo.Object, _password.Object);

            _service = new UserService(
                _uow.Object,
                _mapper.Object,
                _email.Object,
                domain,
                _userLogged.Object,
                _password.Object);
        }

        [When("acionar a função de deletar um usuário")]
        public async Task WhenAcionarAFuncaoDeDeletarUmUsuario()
        {
            try
            {
                await _service.DeleteUser(userRemove.Id);
            }
            catch (UnauthorizedAccessException ex)
            {

                _unauthorizedException = ex;
            }
        }

        [Then("a função deverá lançar uma exceção de não autorizado")]
        public void ThenAFuncaoDeveraLancarUmaExcecaoDeNaoAutorizado()
        {
            Assert.NotNull(_unauthorizedException);
            Assert.IsType<UnauthorizedAccessException>(_unauthorizedException);
        }

    }
}
