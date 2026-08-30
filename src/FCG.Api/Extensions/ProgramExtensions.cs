using AutoMapper;
using FCG.Api.Filters;
using FCG.Api.Middlewares;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.Mapper;
using FCG.Application.Services;
using FCG.Application.Validator;
using FCG.Domain.Contants;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.Respositories;
using FCG.Domain.Services;
using FCG.Infrastructure.Authentication;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Log;
using FCG.Infrastructure.Messaging;
using FCG.Infrastructure.Password;
using FCG.Infrastructure.Persistence;
using FCG.Infrastructure.Repositories;
using FCG.Infrastructure.Settings;
using FCG.Shared.Events;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using System;
using System.Text;

namespace FCG.Api.Extensions;

public static class ProgramExtensions
{
    public static IServiceCollection ConfigureApi(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddScoped<FluentValidationActionFilter>();

        services.ConfigureAuthentication(configuration);

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
            options.OperationFilter<CorrelationIdHeaderFilter>();
        });
        services.AddOpenTelemetry()
        .WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("fcg-users-api"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(options =>
                {
                    // Tempo endpoint (OTLP gRPC)
                    options.Endpoint = new Uri(configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo.observability:4317");
                });
        });
        services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>();
        return services;
    }

    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<UserCreate>, UserValidator>();
        services.AddScoped<IValidator<AdminCreate>, AdminValidator>();
        services.AddScoped<IValidator<RequestChangePassword>, ChangePasswordValidator>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAccessService, AccessService>();

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
        services.AddScoped<IAccessDomainService, AccessDomainService>();

        return services;
    }

    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<INotificationPublisher, NotificationPublisher>();

        var rabbitMqConfig = configuration.GetSection("RabbitMQ").Get<RabbitMqSettings>();
        if (rabbitMqConfig == null ||
            string.IsNullOrWhiteSpace(rabbitMqConfig.Host) ||
            string.IsNullOrWhiteSpace(rabbitMqConfig.Username) ||
            string.IsNullOrWhiteSpace(rabbitMqConfig.Password))
        {
            throw new InvalidOperationException("RabbitMQ configuration is missing or invalid. Application cannot start.");
        }

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    host: rabbitMqConfig.Host,
                    virtualHost: rabbitMqConfig.VirtualHost ?? "/",
                    h =>
                    {
                        h.Username(rabbitMqConfig.Username);
                        h.Password(rabbitMqConfig.Password);
                    });

                cfg.Publish<UserCreatedEvent>(p => p.ExchangeType = "topic");

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
        services.AddScoped<IUserLogged, UserLogged>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }

    

    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.MapHealthChecks("/Health", new HealthCheckOptions
        {
            AllowCachingResponses = false,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            }
        });
        app.UseHttpMetrics();
        app.UseMiddleware<CorrelationMiddleware>();
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<LogMiddleware>();

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
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapMetrics();
        return app;
    }

    public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FCGSettings>(configuration);

        return services;
    }

    private static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
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

        services.AddAuthorizationBuilder()
            .AddPolicy(FCGConstant.AdminRole, policy => policy.RequireRole(FCGConstant.AdminRole))
            .AddPolicy(FCGConstant.UserDefault, policy => policy.RequireRole(FCGConstant.UserDefault))
            .AddPolicy(FCGConstant.AdminOrDefault, policy => policy.RequireRole(FCGConstant.AdminRole, FCGConstant.UserDefault));

        return services;
    }

    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
        return app;
    }

}