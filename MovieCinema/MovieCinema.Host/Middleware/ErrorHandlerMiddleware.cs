using System.Net;
using Newtonsoft.Json;

namespace MovieCinema.Host.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
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
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    AppException e => (int)HttpStatusCode.BadRequest, //custom application error
                    KeyNotFoundException e => (int)HttpStatusCode.NotFound, //not found error
                    _ => (int)HttpStatusCode.InternalServerError //unhandled error
                };

                var result = JsonConvert.SerializeObject(new { message = error.Message, trace = error.StackTrace });
                _logger.LogError(result);

                await response.WriteAsync(result);
            }
        }
    }
}