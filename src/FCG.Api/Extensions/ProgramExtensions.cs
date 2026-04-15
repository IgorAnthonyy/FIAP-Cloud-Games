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
using FCG.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FCG.Domain.Contants;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;

namespace FCG.Api.Extensions;

public static class ProgramExtensions
{
    public static IServiceCollection ConfigureApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<FluentValidationActionFilter>();
        var key = configuration["Jwt:Key"];
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(FCGConstant.AdminRole, policy => policy.RequireRole(FCGConstant.AdminRole));
            options.AddPolicy(FCGConstant.UserDefault, policy => policy.RequireRole(FCGConstant.UserDefault));
        });
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

            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });
        });

        return services;
    }

    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<UserCreate>, UserValidator>();
        services.AddScoped<IValidator<AdminCreate>, AdminValidator>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAcessService, AcessService>();
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
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserDomainService, UserDomainService>();
        services.AddScoped<IAcessDomainService, AcessDomainService>();
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
        services.AddScoped<ITokenService, TokenService>();
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