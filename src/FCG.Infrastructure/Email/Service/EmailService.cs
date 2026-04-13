using FCG.Domain.Enums;
using FCG.Domain.Interfaces;
using FCG.Domain.Views;
using HandlebarsDotNet;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Email.Service;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly IHostEnvironment _env;
    public EmailService(IOptions<EmailSettings> options, IHostEnvironment env)
    {
        _settings = options.Value;
        _env = env;
    }

    private static async Task<string> GetTemplate(string fileName, object data)
    {
        string path = Path.Combine(
            AppContext.BaseDirectory,
            "Email",
            "Hbs",
            $"{fileName}.hbs"
            );

        var origem = await File.ReadAllTextAsync(path);

        var template = Handlebars.Compile(origem);

        return template(data);
    }
    
    public async Task<bool> SendAsync(UserView user, string password = null, EmailOptions option = EmailOptions.Padrao)
    {
        if (option == EmailOptions.Admin && string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Senha temporária obrigatória para envio de e-mail de administrador.", nameof(password));

        var mail = new MailMessage();
        mail.From = new MailAddress(_settings.User);
        mail.To.Add(user.Email);
        mail.Subject = option == EmailOptions.Admin
            ? "Bem-vindo(a) ao sistema Fiap Cloud Games - senha temporária"
            : "Bem-vindo(a) ao sistema Fiap Cloud Games";
        mail.IsBodyHtml = true;

        var templateName = option == EmailOptions.Admin ? "email-admin" : "email";
        object templateData;

        if (option == EmailOptions.Admin)
        {
            templateData = new
            {
                user.Name,
                TemporaryPassword = password
            };
        }
        else
        {
            templateData = new
            {
                user.Name,
                Body = "Bem-vindo(a) ao sistema que vai levar você a diversas jornadas e experiências gamer."
            };
        }

        mail.Body = await GetTemplate(templateName, templateData);
        var smtp = new SmtpClient(_settings.Host)
        {
            Port = _settings.Port,
            Credentials = new NetworkCredential(_settings.User, _settings.Password),
            EnableSsl = true,
        };
        await smtp.SendMailAsync(mail);
        return true;
    }
}