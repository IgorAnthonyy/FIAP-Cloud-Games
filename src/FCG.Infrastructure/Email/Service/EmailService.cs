using FCG.Application.Interfaces;
using FCG.Application.ViewModels;
using HandlebarsDotNet;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Infrastructure.EmailHelper.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IHostEnvironment _env;
        public EmailService(IOptions<EmailSettings> options, IHostEnvironment env)
        {
            _settings = options.Value;
            _env = env;
        }
        private async Task<string> PegarTemplate(string fileName, object data)
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
        public async Task<bool> SendAsync(UserViewModel user, string password = null, EmailOptions option = EmailOptions.Padrao)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_settings.User);
            mail.To.Add(user.Email);
            mail.Subject = "Bem vindo(a) ao sistema FCG";
            mail.IsBodyHtml = true;
            mail.Body = await PegarTemplate("email", new
            {
                Name = user.Name,
                Body = option == EmailOptions.Padrao ? "Bem vindo ao sistema que fará você se aventurar em diversas jornadas e ser o verdadeiro gamer"
                : 
                $@"Bem vindo ao sistema que fará você se aventurar em diversas jornadas e ser o verdadeiro gamer, mas antes disso entre na sua conta com essa senha: {password}"
            });
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
}
