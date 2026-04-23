using FCG.Infrastructure.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FCG.Api.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _log;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var startExecution = DateTime.Now;
            await _next(context);
            var duration = DateTime.Now - startExecution;
            _log.LogInformation("Request {Method} {Path} => StatusCode: {StatusCode} | Duração: {Duration}ms", 
                context.Request.Method, 
                context.Request.Path,
                context.Response.StatusCode,
                duration.TotalMilliseconds);
        }
    }
}
