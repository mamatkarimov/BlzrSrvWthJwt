using Backend.API.Models;
using BSMed.Shared;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Backend.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                await HandleApiExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleUnknownExceptionAsync(context, ex);
            }
        }

        private async Task HandleApiExceptionAsync(HttpContext context, ApiException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.StatusCode;

            var response = new ApiResponse<object>
            {
                Success = false,
                Error = exception.Message,
                Errors = exception.ErrorDetails != null
                    ? new List<string> { exception.ErrorDetails }
                    : new List<string>()
            };

            if (exception is BSMed.Shared.ValidationException)
            {
                // Handle validation errors (e.g., ModelState errors)
                response.Errors = exception.ErrorDetails.Split(',').ToList();
            }

            _logger.LogError(exception, "API Error: {Message}", exception.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private async Task HandleUnknownExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ApiResponse<object>
            {
                Success = false,
                Error = "An unexpected error occurred",
                Errors = new List<string> { exception.Message }
            };

            _logger.LogError(exception, "Unhandled Exception: {Message}", exception.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
