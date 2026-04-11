using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Application.ViewModels;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Tests.Fixture;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.Service
{
    public class UserServiceTest
    {

        private User GetUsuario(string email)
        {
            return new User
            {
                CpfNumber = "00000000",
                BirthDate = DateTime.Now,
                Email = email,
                Name = "name",
                Password = "asdasdasdasdasdadasdasd",
                Phone = "1234567890"
            };
        }
        [Fact]
        public async Task UserEntity_Should_AddedInDb()
        {
            //Arrange
            User usuarioASerCriado = GetUsuario("teste@email.com");
            var uowMock = new Mock<IUnitOfWork>();
            var emailMock = new Mock<IEmailService>();
            var passwordServiceMock = new Mock<IPasswordHashService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var roleRepositoryMock = new Mock<IRoleRepository>();
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<UserDTO>>();

            userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                              .ReturnsAsync(null as User);
            emailMock.Setup(e => e.EnviarEmail(It.IsAny<UserViewModel>())).ReturnsAsync(true);
            userRepositoryMock
                .Setup(r => r.Insert(It.IsAny<User>()))
                .ReturnsAsync(GetUsuario("teste@email.com"));

            validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UserDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            mapperMock.Setup(u => u.Map<User>(It.IsAny<UserDTO>())).Returns(GetUsuario("teste@email.com"));
            mapperMock.Setup(u => u.Map<UserViewModel>(It.IsAny<User>())).Returns(new UserViewModel
            {
                Email = "teste@email.com",
                Name = "name"
            });
            passwordServiceMock.Setup(p => p.GenerateHash(It.IsAny<string>()))
                .Returns("asdasdasdasdasdadasdasd");

            var userService = new UserService(uowMock.Object, passwordServiceMock.Object, userRepositoryMock.Object, validatorMock.Object, mapperMock.Object, roleRepositoryMock.Object, emailMock.Object);


            //Act
            var userCriado = await userService.CriarUsuario(new UserDTO
            {
                CpfNumber = "00000000",
                BirthDate = DateTime.Now,
                Email = "teste@email.com",
                Name = "name",
                Password = "@T1password",
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
            User usuarioASerCriado = GetUsuario("teste");
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
            emailMock.Setup(e => e.EnviarEmail(It.IsAny<UserViewModel>())).ReturnsAsync(true);
            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<UserDTO>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));
            userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                              .ReturnsAsync(null as User);
            userRepositoryMock
                .Setup(r => r.Insert(It.IsAny<User>()))
                .ReturnsAsync(GetUsuario("teste"));
            mapperMock.Setup(u => u.Map<User>(It.IsAny<UserDTO>())).Returns(GetUsuario("teste"));
            mapperMock.Setup(u => u.Map<UserViewModel>(It.IsAny<User>())).Returns(new UserViewModel
            {
                Email = "teste@email.com",
                Name = "name"
            });
            passwordServiceMock.Setup(p => p.GenerateHash(It.IsAny<string>()))
                .Returns("asdasdasdasdasdadasdasd");

            var userService = new UserService(uowMock.Object, passwordServiceMock.Object, userRepositoryMock.Object, validatorMock.Object, mapperMock.Object, roleRepositoryMock.Object, emailMock.Object);


            //Act
            await Assert.ThrowsAsync<ApplicationException>(() => userService.CriarUsuario(new UserDTO
            {
                CpfNumber = "00000000",
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
            User usuarioASerCriado = GetUsuario("teste@email.com");
            var uowMock = new Mock<IUnitOfWork>();
            var emailMock = new Mock<IEmailService>();
            var passwordServiceMock = new Mock<IPasswordHashService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var roleRepositoryMock = new Mock<IRoleRepository>();
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<UserDTO>>();
            emailMock.Setup(e => e.EnviarEmail(It.IsAny<UserViewModel>())).ReturnsAsync(true);
            userRepositoryMock.Setup(u => u.GetByEmail(It.IsAny<string>()))
                              .ReturnsAsync(GetUsuario("teste@email.com"));
            userRepositoryMock
                .Setup(r => r.Insert(It.IsAny<User>()))
                .ReturnsAsync(GetUsuario("teste@email.com"));

            validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UserDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            mapperMock.Setup(u => u.Map<User>(It.IsAny<UserDTO>())).Returns(GetUsuario("teste@email.com"));
            mapperMock.Setup(u => u.Map<UserViewModel>(It.IsAny<User>())).Returns(new UserViewModel
            {
                Email = "teste@email.com",
                Name = "name"
            });
            passwordServiceMock.Setup(p => p.GenerateHash(It.IsAny<string>()))
                .Returns("asdasdasdasdasdadasdasd");

            var userService = new UserService(uowMock.Object, passwordServiceMock.Object, userRepositoryMock.Object, validatorMock.Object, mapperMock.Object, roleRepositoryMock.Object, emailMock.Object);


            //Act
            await Assert.ThrowsAsync<ApplicationException>(() => userService.CriarUsuario(new UserDTO
            {
                CpfNumber = "00000000",
                BirthDate = DateTime.Now,
                Email = "teste",
                Name = "name",
                Password = "12345678",
                Phone = "1234567890"

            }));
        }
    }
}
