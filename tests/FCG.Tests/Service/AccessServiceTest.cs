using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Respositories;
using FCG.Domain.Services;
using FCG.Infrastructure.Password;
using FCG.Tests.Fixture;
using Moq;

namespace FCG.Tests.Service
{

    [Collection(nameof(UserFixtureCollection))]
    public class AccessServiceTest
    {
        public UserFixture _userFixture;

        public AccessServiceTest(UserFixture userFixture)
        {
            _userFixture = userFixture;
        }

        [Fact]
        public async Task AccessService_Should_LoginSucessAndReturnToken()
        {
            //Arrange

            var passwordServiceMock = new PasswordService();
            string passwordTest = "!Teste123456787";
            string hash = passwordServiceMock.GenerateHash(passwordTest);
            User userToLogin = _userFixture.GenerateUserLogin(hash);
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                         .ReturnsAsync(userToLogin);

            var accessDomainService = new AccessDomainService(userRepositoryMock.Object, passwordServiceMock);
            var token = await accessDomainService.Login(userToLogin.Email, new Domain.ValueObjects.Password(passwordTest));

            Assert.IsType<User>(token);
        }
    }
}
