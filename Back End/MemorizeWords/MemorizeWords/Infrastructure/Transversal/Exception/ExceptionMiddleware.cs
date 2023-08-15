using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;
using System.Net;
using System.Text.Json;

namespace MemorizeWords.Infrastructure.Transversal.Exception
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public IApplicationLogger _logger { get; set; }

        public ExceptionMiddleware(RequestDelegate next, IApplicationLogger logger)
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
                _logger.LogException(ex,"Exception Middleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            HttpStatusCode status;
            var stackTrace = string.Empty;
            string message;
            string userMessage;

            var exceptionType = exception.GetType();

            if (exceptionType == typeof(BusinessException))
            {
                message = exception.Message;
                status = HttpStatusCode.InternalServerError;
                stackTrace = exception.StackTrace;
                userMessage = exception.Message;
            }
            else if (exceptionType == typeof(KeyNotFoundBusinessException))
            {
                status = HttpStatusCode.NotImplemented;
                message = exception.Message;
                stackTrace = exception.StackTrace;
                userMessage = exception.Message;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                stackTrace = exception.StackTrace;
                userMessage = "Unknown Error Happened. Please Try Again Later";
            }

            var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace, userMessage });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
