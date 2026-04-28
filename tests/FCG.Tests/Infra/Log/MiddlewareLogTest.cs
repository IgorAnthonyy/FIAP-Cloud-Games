using FCG.Api.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.Tests.Infra.Log;

public class MiddlewareLogTest
{

    [Fact]
    public async Task MiddlewareLog_Should_LogInformation_Request()
    {

        var loggerMock = new Mock<ILogger<LogMiddleware>>();

        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/teste";

        var middleware = new LogMiddleware(
            async (ctx) => ctx.Response.StatusCode = 200,
            loggerMock.Object);

        await middleware.InvokeAsync(context);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);

    }
}
