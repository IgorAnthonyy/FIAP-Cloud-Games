using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FCG.Infrastructure.Settings;

namespace FCG.Api.Extensions;

public static class ProgramExtensions
{
    public static IServiceCollection ConfigureApi(this IServiceCollection services)
    {
        services.AddControllers();

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

    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        // services.AddScoped<IUserRepository, UserRepository>();
        // Injeção de dependência para a camada de infrastructure

        return services;
    }

    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // configuração com base na variável de ambiente
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