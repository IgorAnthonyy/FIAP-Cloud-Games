using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Application.ViewModels;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.Services;
using FCG.Domain.ValueObjects;
using FCG.Tests.Fixture;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using FCG.FCGException.Exceptions;

namespace FCG.Tests.Service;

public class UserServiceTest
{

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
            var validatorMock = new Mock<IValidator<UserDTO>>();

        userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                          .ReturnsAsync(null as User);
        emailMock.Setup(e => e.SendAsync(It.IsAny<UserViewModel>())).ReturnsAsync(true);
        userRepositoryMock
            .Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(GetUser("teste@email.com"));

        validatorMock
        .Setup(v => v.ValidateAsync(It.IsAny<UserDTO>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        mapperMock.Setup(u => u.Map<User>(It.IsAny<UserDTO>())).Returns(GetUser("teste@email.com"));
        mapperMock.Setup(u => u.Map<UserViewModel>(It.IsAny<User>())).Returns(new UserViewModel
        {
            Email = "teste@email.com",
            Name = "name"
        });
        passwordServiceMock.Setup(p => p.GenerateHash(It.IsAny<string>()))
            .Returns("asdasdasdasdasdadasdasd");

        var userDomainService = new UserDomainService(userRepositoryMock.Object, passwordServiceMock.Object);

        var userService = new UserService(uowMock.Object, validatorMock.Object, mapperMock.Object, emailMock.Object, userDomainService);


            //Act
            var userCriado = await userService.CreateUser(new UserDTO
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
    public async Task UserEntity_ShouldThrow_ValidationException()
    {
        //Arrange
        User usuarioASerCriado = GetUser("teste");
        var uowMock = new Mock<IUnitOfWork>();
        var passwordServiceMock = new Mock<IPasswordHashService>();
        var emailMock = new Mock<IEmailService>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var roleRepositoryMock = new Mock<IRoleRepository>();
        var mapperMock = new Mock<IMapper>();
        var validatorMock = new Mock<IValidator<UserDTO>>();
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Password", "Senha inválida")
            };
        emailMock.Setup(e => e.SendAsync(It.IsAny<UserViewModel>())).ReturnsAsync(true);
        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UserDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));
        userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                          .ReturnsAsync(null as User);
        userRepositoryMock
            .Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(GetUser("teste"));
        mapperMock.Setup(u => u.Map<User>(It.IsAny<UserDTO>())).Returns(GetUser("teste"));
        mapperMock.Setup(u => u.Map<UserViewModel>(It.IsAny<User>())).Returns(new UserViewModel
        {
            Email = "teste@email.com",
            Name = "name"
        });
        passwordServiceMock.Setup(p => p.GenerateHash(It.IsAny<string>()))
            .Returns("asdasdasdasdasdadasdasd");

        var userDomainService = new UserDomainService(userRepositoryMock.Object, passwordServiceMock.Object);

        var userService = new UserService(uowMock.Object, validatorMock.Object, mapperMock.Object, emailMock.Object, userDomainService);

            //Act
            await Assert.ThrowsAsync<BusinessException>(() => userService.CreateUser(new UserDTO
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
        var validatorMock = new Mock<IValidator<UserDTO>>();
        emailMock.Setup(e => e.SendAsync(It.IsAny<UserViewModel>())).ReturnsAsync(true);
        userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                          .ReturnsAsync(GetUser("teste@email.com"));
        userRepositoryMock
            .Setup(r => r.Insert(It.IsAny<User>()))
            .ReturnsAsync(GetUser("teste@email.com"));

        validatorMock
        .Setup(v => v.ValidateAsync(It.IsAny<UserDTO>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        mapperMock.Setup(u => u.Map<User>(It.IsAny<UserDTO>())).Returns(GetUser("teste@email.com"));
        mapperMock.Setup(u => u.Map<UserViewModel>(It.IsAny<User>())).Returns(new UserViewModel
        {
            Email = "teste@email.com",
            Name = "name"
        });
        passwordServiceMock.Setup(p => p.GenerateHash(It.IsAny<string>()))
            .Returns("asdasdasdasdasdadasdasd");

        var userDomainService = new UserDomainService(userRepositoryMock.Object, passwordServiceMock.Object);

        var userService = new UserService(uowMock.Object, validatorMock.Object, mapperMock.Object, emailMock.Object, userDomainService);

            //Act
            await Assert.ThrowsAsync<Exception>(() => userService.CreateUser(new UserDTO
            {
                Cpf = "00000000",
                BirthDate = DateTime.Now,
                Email = "teste",
                Name = "name",
                Password = "12345678",
                Phone = "1234567890"

        }));
    }
}
