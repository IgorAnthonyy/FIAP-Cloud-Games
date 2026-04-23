using Castle.Core.Logging;
using FCG.Api.Middlewares;
using FCG.Domain.Views;
using FCG.Infrastructure.Email.Service;
using FCG.Infrastructure.Log;
using FCG.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.Infra.Log
{
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
}
