using BeautyMap.Common.ErrorHandler.Exceptions;
using BeautyMap.Common.ErrorHandler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace BeautyMap.Common.ErrorHandler.Middlware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var error = new ErrorResponse
            {
                Instance = context.Request.Path
            };

            switch (exception)
            {
                case NotFoundException:
                    error.Status = (int)HttpStatusCode.NotFound;
                    error.Title = "Resource Not Found";
                    break;

                case AppValidationException validationEx:
                    error.Status = (int)HttpStatusCode.BadRequest;
                    error.Title = "Validation Failed";
                    error.Extensions["errors"] = validationEx.Errors;
                    break;

                default:
                    error.Status = (int)HttpStatusCode.InternalServerError;
                    error.Title = "Internal Server Error";
                    logger.LogError(exception, "Unhandled exception");
                    break;
            }

            var json = JsonSerializer.Serialize(error);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);

        }
    }
}
