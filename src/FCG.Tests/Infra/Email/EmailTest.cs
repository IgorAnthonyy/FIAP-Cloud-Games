using FCG.Infrastructure.Email.Service;
using FCG.Domain.Views;
using Microsoft.Extensions.Options;
using FCG.Infrastructure.Settings;

namespace FCG.Tests.Infra.Email;

public class EmailTest
{
    [Fact]
    public async Task EmailService_Should_EmailPadrao()
    {
        var settings = Options.Create(new FCGSettings
        {
            EmailSettings = new EmailSettings
            {
                Host = "smtp.gmail.com",
                Port = 587,
                User = "grupofiap77@gmail.com",
                Password = "jleh ckhf zqjt vgho"
            }
        });
    
        //Arrange
        var emailService = new EmailService(settings);

        //Act
        bool send = await emailService.SendAsync(new UserView
        {
            Email = "kingolo0102@gmail.com",
            Name = "test"
        });

        Assert.True(send);
    }
}

