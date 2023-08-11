using System.Net;

namespace App.Middleware
{
    public class ExceptionsHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionsHandlerMiddleware> _logger;

        public ExceptionsHandlerMiddleware(ILogger<ExceptionsHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var request = context.Request;
            var methodType = request.Method;
            var path = request.Path;
            var controllerName = context.GetRouteValue("controller")?.ToString();
            var actionName = context.GetRouteValue("action")?.ToString();

            _logger.LogError(exception, "Необработанное исключение. {Controller}Controller/{Action}: [{Method}] {Path}", controllerName, actionName, methodType, path);

            var statusCode = HttpStatusCode.InternalServerError;
            if (exception is UnauthorizedAccessException)
                statusCode = HttpStatusCode.Unauthorized;

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync("Произошла неизвестная ошибка при обработке запроса. Пожалуйста, сообщите разработчикам.");
        }
    }
}
