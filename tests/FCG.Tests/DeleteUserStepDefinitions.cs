using AutoMapper;
using CommonTestUtilities.Entities;
using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.Respositories;
using FCG.Domain.Services;
using FCG.Tests.Fixture;
using Moq;
using Reqnroll;
using Reqnroll.Assist;
using System;

namespace FCG.Tests
{
    [Collection(nameof(UserFixtureCollection))]
    [Binding]
    public class DeleteUserStepDefinitions
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
        public DeleteUserStepDefinitions(UserBuilder userFixture, AdminContext context)
        {
            _userFixture = userFixture;
            _userLogged = context.UserLogged;
            _userRepo = context.UserRepo;
            userAdmin = context.UserAdmin;
        }
        

        [Given("passo um usuário que existe no sistema")]
        public void GivenPassoUmUsuarioQueExisteNoSistema()
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
                _notificationPublisher.Object,
                domain,
                _userLogged.Object,
                _password.Object);

        }

        [When("executar a função de deletar um usuário")]
        public async Task WhenExecutarAFuncaoDeDeletarUmUsuario()
        {
            await _service.DeleteUser(userRemove.Id);
        }

        [Then("o usuário deve ser excluido")]
        public void ThenOUsuarioDeveSerExcluido()
        {
            _uow.Verify(x => x.CommitAsync(), Times.Once);
        }

    }
}
