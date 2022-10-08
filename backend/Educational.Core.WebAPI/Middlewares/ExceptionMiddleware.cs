using Educational.Core.BLL.Exceptions;
using Newtonsoft.Json;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Educational.Core.WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case NotFoundException:
                        await HandleException(httpContext, ex, HttpStatusCode.NotFound);
                        break;
                    case NotVerifiedException:
                        await HandleException(httpContext, ex, HttpStatusCode.BadRequest);
                        break;
                    case InvalidPasswordException:
                        await HandleException(httpContext, ex, HttpStatusCode.BadRequest);
                        break;
                    default:
                        await HandleException(httpContext, ex, HttpStatusCode.InternalServerError);
                        break;
                }
            }
        }

        private static async Task CreateExceptionResponseAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            object? errorBody = null)
        {
            errorBody ??= new { error = "Unknown error has occured" };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorBody));
        }

        private async Task HandleException(
            HttpContext context,
            Exception ex,
            HttpStatusCode statusCode)
        {
            _logger.Error("{Message}", ex.Message);
            if (ex.InnerException is not null)
            {
                _logger.Error("{Message}", ex.InnerException.Message);
            }

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                await CreateExceptionResponseAsync(context, statusCode);
            }
            else
            {
                await CreateExceptionResponseAsync(context, statusCode, new { error = ex.Message });
            }

        }
    }
}
