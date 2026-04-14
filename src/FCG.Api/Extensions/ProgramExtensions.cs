using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mapper;
using FCG.Application.Services;
using FCG.Application.Validator;
using FCG.Domain.Interfaces;
using FCG.Domain.Services;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Persistence;
using FCG.Infrastructure.Repositories;
using FCG.Infrastructure.Settings;
using FCG.Api.Middlewares;
using FCG.Api.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using FCG.Infrastructure.Email.Service;
using FCG.Domain.Interfaces.Respositories;
using FCG.Infrastructure.Password;
using FCG.Infrastructure.Security;

namespace FCG.Api.Extensions;

public static class ProgramExtensions
{
    public static IServiceCollection ConfigureApi(this IServiceCollection services)
    {
        services.AddScoped<FluentValidationActionFilter>();

        services.AddControllers(options =>
        {
            options.Filters.Add<FluentValidationActionFilter>();
        });
        services.AddHttpContextAccessor();
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
        services.AddScoped<IValidator<UserCreate>, UserValidator>();
        services.AddScoped<IValidator<AdminCreate>, AdminValidator>();
        
        services.AddScoped<IUserService, UserService>();
        
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

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserLogged, UserLogged>();
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

        return services;
    }
}