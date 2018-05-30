using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SchedulingApp.Infrastucture.Middleware.Exception;

namespace SchedulingApp.Infrastucture.Middlewares.Exception
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, ExceptionMiddleware will not be executed.");
                    throw;
                }

                context.Response.Clear();

                if (ex is UseCaseException uc)
                {
                    context.Response.StatusCode = (int) uc.StatusCode;
                    context.Response.ContentType = uc.ContentType;
                    await context.Response.WriteAsync(uc.Message);
                }
                else
                {
                    _logger.LogInformation("Unhandled expection has been catched.");
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                }
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}