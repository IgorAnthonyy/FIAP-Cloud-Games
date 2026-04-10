using FCG.Api.Extensions;
using FCG.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCG.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Services

        builder.Services
            .ConfigureSettings(builder.Configuration)
            .ConfigureApi()
            .ConfigureApplication()
            .ConfigureDomain()
            .ConfigureInfrastructure(builder.Configuration)

        #endregion

        var app = builder.Build();

        #region Middleware

        app.ConfigureMiddleware();

        #endregion

        app.Run();

    }
}