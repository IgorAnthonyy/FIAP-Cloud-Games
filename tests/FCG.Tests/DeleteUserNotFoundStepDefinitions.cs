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
    public class DeleteUserNotFoundStepDefinitions
    {
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IEmailService> _email = new();
        private readonly Mock<IPasswordService> _password = new();
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IUserLogged> _userLogged = new();
        public UserBuilder _userFixture;
        private UserService _service;
        private User userAdmin;
        private User userRemove;
        private BusinessException _businessException;
        public DeleteUserNotFoundStepDefinitions(UserBuilder userFixture, AdminContext context)
        {
            _userFixture = userFixture;
            _userRepo = context.UserRepo;
            _userLogged = context.UserLogged;
            userAdmin = context.UserAdmin;
        }

        

        [Given("passo um usuário que não existe no sistema")]
        public void GivenPassoUmUsuarioQueNaoExisteNoSistema()
        {

            userRemove = _userFixture.GenerateUserWithRoleEmpty();
            _userRepo.Setup(u => u.GetById(It.IsAny<Guid>()))
                          .ReturnsAsync(null as User);

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

        [When("rodar a função de deletar um usuário")]
        public async Task WhenRodarAFuncaoDeDeletarUmUsuario()
        {
            try
            {
                await _service.DeleteUser(userRemove.Id);
            }
            catch (BusinessException ex)
            {

                _businessException = ex;
            }
        }

        [Then("a função deverá lançar uma exceção")]
        public void ThenAFuncaoDeveraLancarUmaExcecao()
        {
            Assert.NotNull(_businessException);
            Assert.IsType<BusinessException>(_businessException);
        }

    }
}
