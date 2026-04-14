using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enums;
using FCG.Domain.Exceptions;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.Respositories;
using FCG.Domain.Services;
using FCG.Domain.ValueObjects;
using FCG.Domain.Views;
using FCG.Tests.Fixture;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;
using System.Security.Claims;

namespace FCG.Tests.Service;

[Collection(nameof(UserFixtureCollection))]
public class UserServiceTest
{

    public UserFixture _userFixture;

    public UserServiceTest(UserFixture userFixture)
    {
        _userFixture = userFixture;
    }
    private User GetUser(string email)
        {
            return new User
            {
                Cpf = new CPF("51619938049"),
                BirthDate = DateTime.Now,
                Email = new Email("teste@teste.com"),
                Name = "name",
                Password = "123@T1password",
                Phone = "1234567890"
               
            };
        }

    [Fact]
    public async Task UserEntity_Should_AddedInDb()
    {
        //Arrange
        User usuarioASerCriado = GetUser("teste@email.com");
        var uowMock = new Mock<IUnitOfWork>();
        var emailMock = new Mock<IEmailService>();
        var passwordServiceMock = new Mock<IPasswordHashService>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var roleRepositoryMock = new Mock<IRoleRepository>();
        var mapperMock = new Mock<IMapper>();
        var userLoggedMock = new Mock<IUserLogged>();

        userLoggedMock.Setup(ul => ul.UserEmail).Returns("");
        userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                          .ReturnsAsync(null as User);
        emailMock.Setup(e => e.SendAsync(It.IsAny<UserView>())).ReturnsAsync(true);
        userRepositoryMock
            .Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(GetUser("teste@email.com"));

        mapperMock.Setup(u => u.Map<User>(It.IsAny<UserCreate>())).Returns(GetUser("teste@email.com"));
        mapperMock.Setup(u => u.Map<UserView>(It.IsAny<User>())).Returns(new UserView
        {
            Email = "teste@email.com",
            Name = "name"
        });
        mapperMock.Setup(u => u.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
        {
            Email = "teste@email.com",
            Name = "name"
        });
        passwordServiceMock.Setup(p => p.GenerateHash(It.IsAny<string>()))
            .Returns("asdasdasdasdasdadasdasd");

        var userDomainService = new UserDomainService(userRepositoryMock.Object, passwordServiceMock.Object);

        var userService = new UserService(uowMock.Object, mapperMock.Object, emailMock.Object, userDomainService, userLoggedMock.Object);


            //Act
        var userCriado = await userService.CreateUser(new UserCreate
        {
            Cpf = "00000000",
            BirthDate = DateTime.Now,
            Email = "teste@email.com",
            Name = "name",
            Password = "123@T1password",
            Phone = "1234567890"

        });

        userRepositoryMock.Verify(r => r.Insert(It.IsAny<User>()), Times.Once);
        uowMock.Verify(u => u.CommitAsync(), Times.Once);
        Assert.Equal("name", userCriado.Name);
    }


    [Fact]
    public async Task UserEntityLoggedAdmin_Should_RemovedUserInDb()
    {
        //Arrange
        User userAdmin = _userFixture.GenerateUserWithRoleAdmin();
        User userRemove = _userFixture.GenerateUserWithRoleEmpty();
        var uowMock = new Mock<IUnitOfWork>();
        var emailMock = new Mock<IEmailService>();
        var passwordServiceMock = new Mock<IPasswordHashService>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var roleRepositoryMock = new Mock<IRoleRepository>();
        var mapperMock = new Mock<IMapper>();
        var userLoggedMock = new Mock<IUserLogged>();

        userLoggedMock.Setup(ul => ul.UserEmail).Returns(userAdmin.Email.Value);


        userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                          .ReturnsAsync(userAdmin);

        userRepositoryMock.Setup(u => u.GetById(It.IsAny<Guid>()))
                          .ReturnsAsync(userRemove);

        userRepositoryMock
            .Setup(r => r.Delete(It.IsAny<User>()))
            .Returns(userRemove);


        var userDomainService = new UserDomainService(userRepositoryMock.Object, passwordServiceMock.Object);

        var userService = new UserService(uowMock.Object, mapperMock.Object, emailMock.Object, userDomainService, userLoggedMock.Object);

        //Act
        bool userDeleted = await userService.DeleteUser(userRemove.Id);

        userRepositoryMock.Verify(r => r.Delete(It.IsAny<User>()), Times.Once);
        uowMock.Verify(u => u.CommitAsync(), Times.Once);
        Assert.True(userDeleted);
    }


    [Fact]
    public async Task UserEntityLoggedAdmin_ShouldThrow_RemovedUserNotFoundException()
    {
        //Arrange
        User userAdmin = _userFixture.GenerateUserWithRoleAdmin();
        User userRemove = _userFixture.GenerateUserWithRoleEmpty();
        var uowMock = new Mock<IUnitOfWork>();
        var emailMock = new Mock<IEmailService>();
        var passwordServiceMock = new Mock<IPasswordHashService>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var roleRepositoryMock = new Mock<IRoleRepository>();
        var mapperMock = new Mock<IMapper>();
        var userLoggedMock = new Mock<IUserLogged>();

        userLoggedMock.Setup(ul => ul.UserEmail).Returns(userAdmin.Email.Value);
        userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                          .ReturnsAsync(userAdmin);

        userRepositoryMock.Setup(u => u.GetById(It.IsAny<Guid>()))
                          .ReturnsAsync(null as User);

        userRepositoryMock
            .Setup(r => r.Delete(It.IsAny<User>()))
            .Returns(userRemove);


        var userDomainService = new UserDomainService(userRepositoryMock.Object, passwordServiceMock.Object);

        var userService = new UserService(uowMock.Object, mapperMock.Object, emailMock.Object, userDomainService, userLoggedMock.Object);


        //Act
        await Assert.ThrowsAsync<BusinessException>(() => userService.DeleteUser(userRemove.Id));
    }

    [Fact]
    public async Task UserEntityLoggedDefault_ShouldThrow_NotAuthorizatedException()
    {
        //Arrange
        User userAdmin = _userFixture.GenerateUserWithRolesDefault();
        User userRemove = _userFixture.GenerateUserWithRoleEmpty();
        var uowMock = new Mock<IUnitOfWork>();
        var emailMock = new Mock<IEmailService>();
        var passwordServiceMock = new Mock<IPasswordHashService>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var roleRepositoryMock = new Mock<IRoleRepository>();
        var mapperMock = new Mock<IMapper>();
        var userLoggedMock = new Mock<IUserLogged>();

        userLoggedMock.Setup(ul => ul.UserEmail).Returns(userAdmin.Email.Value);
        userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                          .ReturnsAsync(userAdmin);
        userRepositoryMock.Setup(u => u.GetById(It.IsAny<Guid>()))
                          .ReturnsAsync(userRemove);


        userRepositoryMock
            .Setup(r => r.Delete(It.IsAny<User>()))
            .Returns(userRemove);


        var userDomainService = new UserDomainService(userRepositoryMock.Object, passwordServiceMock.Object);

        var userService = new UserService(uowMock.Object, mapperMock.Object, emailMock.Object, userDomainService, userLoggedMock.Object);


        //Act
        bool userDeleted = await userService.DeleteUser(userRemove.Id);

        Assert.False(userDeleted);
    }

    [Fact]
    public async Task UserEntityLogged_ShouldThrow_NotFoundLoggedUserException()
    {
        //Arrange
        User userAdmin = _userFixture.GenerateUserWithRoles();
        User userRemove = _userFixture.GenerateUserWithRoleEmpty();
        var uowMock = new Mock<IUnitOfWork>();
        var emailMock = new Mock<IEmailService>();
        var passwordServiceMock = new Mock<IPasswordHashService>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var roleRepositoryMock = new Mock<IRoleRepository>();
        var mapperMock = new Mock<IMapper>();
        var userLoggedMock = new Mock<IUserLogged>();

        userLoggedMock.Setup(ul => ul.UserEmail).Returns(userAdmin.Email.Value);
        userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                          .ReturnsAsync(null as User);

        userRepositoryMock.Setup(u => u.GetById(It.IsAny<Guid>()))
                          .ReturnsAsync(userRemove);

        userRepositoryMock
            .Setup(r => r.Delete(It.IsAny<User>()))
            .Returns(userRemove);


        var userDomainService = new UserDomainService(userRepositoryMock.Object, passwordServiceMock.Object);

        var userService = new UserService(uowMock.Object, mapperMock.Object, emailMock.Object, userDomainService, userLoggedMock.Object);


        //Act
        await Assert.ThrowsAsync<BusinessException>(() => userService.DeleteUser(userRemove.Id));
    }


    [Fact]
    public async Task UserEntity_ShouldThrow_FoundUserException()
    {
        //Arrange
        User usuarioASerCriado = GetUser("teste@email.com");
        var uowMock = new Mock<IUnitOfWork>();
        var emailMock = new Mock<IEmailService>();
        var passwordServiceMock = new Mock<IPasswordHashService>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var roleRepositoryMock = new Mock<IRoleRepository>();
        var mapperMock = new Mock<IMapper>();
        var userLoggedMock = new Mock<IUserLogged>();

        userLoggedMock.Setup(ul => ul.UserEmail).Returns("");
        emailMock.Setup(e => e.SendAsync(It.IsAny<UserView>())).ReturnsAsync(true);
        userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                          .ReturnsAsync(GetUser("teste@email.com"));
        userRepositoryMock
            .Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(GetUser("teste@email.com"));

        mapperMock.Setup(u => u.Map<User>(It.IsAny<UserCreate>())).Returns(GetUser("teste@email.com"));
        mapperMock.Setup(u => u.Map<UserView>(It.IsAny<User>())).Returns(new UserView
        {
            Email = "teste@email.com",
            Name = "name"
        });
        mapperMock.Setup(u => u.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
        {
            Email = "teste@email.com",
            Name = "name"
        });
        passwordServiceMock.Setup(p => p.GenerateHash(It.IsAny<string>()))
            .Returns("asdasdasdasdasdadasdasd");

        var userDomainService = new UserDomainService(userRepositoryMock.Object, passwordServiceMock.Object);

        var userService = new UserService(uowMock.Object, mapperMock.Object, emailMock.Object, userDomainService, userLoggedMock.Object);

            //Act
        await Assert.ThrowsAsync<BusinessException>(() => userService.CreateUser(new UserCreate
        {
            Cpf = "00000000",
            BirthDate = DateTime.Now,
            Email = "teste",
            Name = "name",
            Password = "12345678",
            Phone = "1234567890"

        }));
    }

    [Fact]
    public async Task AdminEntity_Should_AddedInDb_WithTemporaryPasswordEmail()
    {
        //Arrange
        var uowMock = new Mock<IUnitOfWork>();
        var emailMock = new Mock<IEmailService>();
        var passwordServiceMock = new Mock<IPasswordHashService>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var mapperMock = new Mock<IMapper>();
        var userLoggedMock = new Mock<IUserLogged>();

        userLoggedMock.Setup(ul => ul.UserEmail).Returns("");
        User? insertedUserCapture = null;

        userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                          .ReturnsAsync(null as User);
        userRepositoryMock
            .Setup(r => r.Insert(It.IsAny<User>()))
            .Callback<User>(u => insertedUserCapture = u)
            .ReturnsAsync((User u) => u);

        mapperMock.Setup(u => u.Map<User>(It.IsAny<AdminCreate>())).Returns((AdminCreate u) => new User
        {
            Cpf = new CPF("51619938049"),
            BirthDate = u.BirthDate,
            Email = new Email(u.Email),
            Name = u.Name,
            Phone = u.Phone
        });

        mapperMock.Setup(u => u.Map<UserResponse>(It.IsAny<User>())).Returns((User u) => new UserResponse
        {
            Name = u.Name,
            Email = u.Email.Value,
            Phone = u.Phone,
            BirthDate = u.BirthDate,
            Cpf = u.Cpf.Code
        });

        mapperMock.Setup(u => u.Map<UserView>(It.IsAny<User>())).Returns((User u) => new UserView
        {
            Name = u.Name,
            Email = u.Email.Value,
            Phone = u.Phone,
            BirthDate = u.BirthDate,
            Cpf = u.Cpf.Code
        });

        emailMock.Setup(e => e.SendAsync(It.IsAny<UserView>(), It.IsAny<string>(), EmailOptions.Admin)).ReturnsAsync(true);
        passwordServiceMock.Setup(p => p.GenerateHash(It.IsAny<string>())).Returns("hashed");

        var userDomainService = new UserDomainService(userRepositoryMock.Object, passwordServiceMock.Object);
        var userService = new UserService(uowMock.Object, mapperMock.Object, emailMock.Object, userDomainService, userLoggedMock.Object);

        //Act
        var userCriado = await userService.CreateAdmin(new AdminCreate
        {
            Cpf = "51619938049",
            BirthDate = DateTime.Now,
            Email = "admin@email.com",
            Name = "admin",
            Phone = "1234567890"
        });

        //Assert
        Assert.NotNull(insertedUserCapture);
        Assert.Contains(insertedUserCapture.Roles, r => r.Name == "ADMIN");
        emailMock.Verify(e => e.SendAsync(It.IsAny<UserView>(), It.Is<string>(s => !string.IsNullOrWhiteSpace(s)), EmailOptions.Admin), Times.Once);
        uowMock.Verify(u => u.CommitAsync(), Times.Once);
        Assert.Equal("admin", userCriado.Name);
    }
}
