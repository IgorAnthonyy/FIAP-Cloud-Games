using FCG.Domain.Interfaces;
using FCG.Infrastructure.Data;
using FCG.Infrastructure.Persistence;
using FCG.Infrastructure.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;

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
        // services.AddScoped<IUserApplicationService, UserApplicationService>();
        // Injeção de dependência para a camada de application

        return services;
    }

    public static IServiceCollection ConfigureDomain(this IServiceCollection services)
    {
        // services.AddScoped<IUserService, UserService>();
        // Injeção de dependência para a camada de domain

        return services;
    }

    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddScoped<IUserRepository, UserRepository>();
        // Injeção de dependência para a camada de infrastructure
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
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