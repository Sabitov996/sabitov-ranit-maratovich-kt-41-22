using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RanitSabitovKt_41_22.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new ResponseModel<object>
                {
                    Succeeded = false,
                    Message = "Произошла ошибка на сервере",
                    Errors = new List<string>
                    {
                        ex.Message,
                        ex.InnerException?.Message ?? "Нет внутренней ошибки"
                    },
                    Data = null
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

    public class ResponseModel<T>
    {
        public bool Succeeded { get; set; }
        public required string Message { get; set; }
        public required List<string> Errors { get; set; }
        public required T Data { get; set; }

        public ResponseModel()
        {
            Message = string.Empty;
            Errors = new List<string>();
            Data = default!;
        }

        public ResponseModel(T data, string? message = null)
        {
            Succeeded = true;
            Message = message ?? string.Empty;
            Errors = new List<string>();
            Data = data;
        }

        public ResponseModel(string message)
        {
            Succeeded = true;
            Message = message ?? string.Empty;
            Errors = new List<string>();
            Data = default!;
        }
    }
}
