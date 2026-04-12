using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mapper;
using FCG.Application.Services;
using FCG.Application.Validator;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.Services;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.PasswordHelper;
using FCG.Infrastructure.Persistence;
using FCG.Infrastructure.Repositories;
using FCG.Infrastructure.Settings;
using FCG.Api.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using FCG.Infrastructure.Email.Service;

namespace FCG.Api.Extensions;

public static class ProgramExtensions
{
    public static IServiceCollection ConfigureApi(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FCG - FIAP Cloud Games API",
                Version = "v1",
                Description = "API desenvolvida para o Tech Challenge - Grupo 77 (12NETT)",
                Contact = new OpenApiContact
                {
                    Name = "FIAP Cloud Games"
                }
            });
        });

        return services;
    }

    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<UserDTO>, UserValidator>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMapper>(); // seus profiles
        }, loggerFactory);

        IMapper mapper = mapperConfig.CreateMapper();

        
        services.AddSingleton(mapper);
        return services;
    }

    public static IServiceCollection ConfigureDomain(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHashService, PasswordHashService>();
        services.AddScoped<IUserDomainService, UserDomainService>();
        return services;
    }

    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }

    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "FCG API v1");
                options.DocumentTitle = "FCG - FIAP Cloud Games API";
            });
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        return app;
    }

    public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FCGSettings>(configuration);
        services.Configure<EmailSettings>(configuration.GetSection("Email"));

        return services;
    }
}