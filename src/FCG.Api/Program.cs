using FCG.Api.Extensions;
using Microsoft.AspNetCore.Builder;

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
            .ConfigureInfrastructure();

        #endregion

        var app = builder.Build();

        #region Middleware

        app.ConfigureMiddleware();

        #endregion

        app.Run();

    }
}