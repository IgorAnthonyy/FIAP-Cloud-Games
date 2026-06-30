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
    public class DeleteYourSelfStepDefinitions
    {

        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<INotificationPublisher> _notificationPublisher = new();
        private readonly Mock<IPasswordService> _password = new();
        private readonly Mock<IUserRepository> _userRepo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IUserLogged> _userLogged = new();
        public UserBuilder _userFixture;
        private UserService _service = null!;
        private User userAdmin;
        private User userRemove = null!;
        private BusinessException _businessException = null!;
        public DeleteYourSelfStepDefinitions(UserBuilder userFixture, AdminContext context)
        {
            _userFixture = userFixture;
            _userLogged = context.UserLogged;
            _userRepo = context.UserRepo;
            userAdmin = context.UserAdmin;
        }

        [Given("me passo para se deletar")]
        public void GivenMePassoParaSeDeletar()
        {
            userRemove = userAdmin;
            _userRepo.Setup(x => x.GetById(userRemove.Id))
                .ReturnsAsync(userRemove);

            _userRepo.Setup(x => x.Delete(It.IsAny<User>()))
                     .Returns(userRemove);

            var domain = new UserDomainService(_userRepo.Object, _password.Object);

            _service = new UserService(
                _uow.Object,
                _mapper.Object,
                _notificationPublisher.Object,
                domain,
                _userLogged.Object,
                _password.Object);
        }

        [When("texto executar a função de deletar um usuário")]
        public async Task WhenTextoExecutarAFuncaoDeDeletarUmUsuario()
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

        [Then("a rota deverá lançar uma exceção")]
        public void ThenARotaDeveraLancarUmaExcecao()
        {
            Assert.NotNull(_businessException);
            Assert.IsType<BusinessException>(_businessException);
        }

    }
}
