using FCG.Application.Interfaces;
using FCG.Infrastructure.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace FCG.Api.Middlewares
{
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _correlationIdHeader = "x-correlation-id";
        private readonly ILogger<CorrelationMiddleware> _logger;
        public CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
        {
            
            var correlationId = GetCorrelationId(context, correlationIdGenerator);
            context.Response.Headers[_correlationIdHeader] = correlationId.ToString();
            using (_logger.BeginScope("[CORRELATION_ID] - {CorrelationID}", correlationId))
            {
                await _next(context);

            }

            
            
        }

        private StringValues GetCorrelationId(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
        {
            if(context.Request.Headers.TryGetValue(_correlationIdHeader, out var idCorrelation))
            {
                correlationIdGenerator.CorrelationId = idCorrelation;
                return idCorrelation;
            }
            else
            {
                idCorrelation = Guid.NewGuid().ToString();
                correlationIdGenerator.CorrelationId = idCorrelation;
                return idCorrelation;
            }
        }

        //private void AddCorrelationIdInHeaderResponse(HttpContext context, StringValues correlationId) => context.Response.OnStarting(() =>
        //{
        //    context.Response.Headers[_correlationIdHeader] = new[] { correlationId.ToString() };
        //    return Task.CompletedTask;
        //});
    }
}
