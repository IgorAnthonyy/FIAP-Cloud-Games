using FCG.Domain.Interfaces;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Respositories;
using FCG.Domain.Services;
using FCG.Tests.Fixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using FCG.Infrastructure.Password;

namespace FCG.Tests.Service
{

    [Collection(nameof(UserFixtureCollection))]
    public class AcessoServiceTest
    {
        public UserFixture _userFixture;

        public AcessoServiceTest(UserFixture userFixture)
        {
            _userFixture = userFixture;
        }

        [Fact]
        public async Task AcessoService_Should_LoginSucessAndReturnToken()
        {
            //Arrange

            var passwordServiceMock = new PasswordService();
            string passwordTest = "!Teste123456787";
            string hash = passwordServiceMock.GenerateHash(passwordTest);
            User userToLogin = _userFixture.GenerateUserLogin(hash);
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                         .ReturnsAsync(userToLogin);

            var acessoDomainService = new AcessDomainService(userRepositoryMock.Object, passwordServiceMock);
            var token = await acessoDomainService.Login(userToLogin.Email, new Domain.ValueObjects.Password(passwordTest));

            Assert.IsType<User>(token);
        }
    }
}
