using FCG.Api.Middlewares;
using FCG.Application.Interfaces;
using FCG.Infrastructure.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.Infra.Log
{
    public class CorrelationIdTest
    {

        [Fact]
        public async Task CorrelationIdMiddlewareGenerator_Should_Generate_Id()
        {
            var loggerMock = new Mock<ILogger<CorrelationMiddleware>>();
            var correlationIdGeneratorMock = new Mock<ICorrelationIdGenerator>();
            correlationIdGeneratorMock.Setup(x => x.CorrelationId).Returns("teste");
            correlationIdGeneratorMock.SetupProperty(x => x.CorrelationId, "teste");
            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/teste";

            var middleware = new CorrelationMiddleware(
                async (ctx) => ctx.Response.StatusCode = 200, loggerMock.Object);

            await middleware.InvokeAsync(context,correlationIdGeneratorMock.Object);

            Assert.IsType<string>(context.Response.Headers["x-correlation-id"].ToString());

            

        }
        [Fact]
        public async Task CorrelationIdMiddlewareGenerator_Should_KeptIdRequest()
        {
            var loggerMock = new Mock<ILogger<CorrelationMiddleware>>();

            var correlationIdGeneratorMock = new Mock<ICorrelationIdGenerator>();
            correlationIdGeneratorMock.Setup(x => x.CorrelationId).Returns("teste");
            correlationIdGeneratorMock.SetupProperty(x => x.CorrelationId, "teste");
            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/teste";
            context.Request.Headers["x-correlation-id"] = "teste";
            var middleware = new CorrelationMiddleware(
                async (ctx) => ctx.Response.StatusCode = 200, loggerMock.Object);

            await middleware.InvokeAsync(context, correlationIdGeneratorMock.Object);

            Assert.IsType<string>(context.Response.Headers["x-correlation-id"].ToString());



        }
    }
}
