using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Tests.Fixture;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.Service
{
    public class UserServiceTest
    {
        [Fact]
        public async Task UserEntity_Should_AddedInDb()
        {
            //Arrange
            var uowMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var roleRepositoryMock = new Mock<IRoleRepository>();
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<UserDTO>>();
            validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UserDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var userService = new UserService(uowMock.Object, userRepositoryMock.Object, validatorMock.Object, mapperMock.Object, roleRepositoryMock.Object);


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
    }
}
