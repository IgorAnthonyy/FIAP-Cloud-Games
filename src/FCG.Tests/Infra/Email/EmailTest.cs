using FCG.Domain.Entities;
using FCG.Infrastructure.Email.Service;
using FCG.Tests.Fixture;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.Infra.Email
{
    public class EmailTest
    {
        [Fact]
        public async Task EmailService_Should_EmailPadrao()
        {

            var mockEnv = new Mock<IHostEnvironment>();

            mockEnv.Setup(e => e.ContentRootPath)
                   .Returns(Directory.GetCurrentDirectory());
            var settings = Options.Create(new EmailSettings
            {
                Host = "smtp.gmail.com",
                Port = 587,
                User = "grupofiap77@gmail.com",
                Password = "jleh ckhf zqjt vgho"
            });

            //Arrange
            var emailService = new EmailService(settings, mockEnv.Object);

            //Act
            bool send = await emailService.SendAsync(new Application.ViewModels.UserViewModel
            {
                Email = "kingolo0102@gmail.com",
                Name = "test"
            });

            Assert.True(send);
        }
    }
}
