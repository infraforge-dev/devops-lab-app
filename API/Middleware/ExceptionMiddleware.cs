using System.Net;
using System.Text.Json;
using API.ExceptionResponse;
using Core.ExceptionTypes;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly IHostEnvironment _env;
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="env">The hosting environment.</param>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger instance.</param>
        /// <remarks>
        /// The traditional constructor pattern is used here instead of a primary constructor
        /// to ensure compatibility across all supported C# versions, make dependencies explicit,
        /// and follow established .NET middleware conventions.
        /// </remarks>
        public ExceptionMiddleware(IHostEnvironment env, RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _env = env;
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string message;
            string? details = null;

            switch (ex)
            {
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound; // 404
                    message = ex.Message;
                    break;

                case ValidationException vex:
                    statusCode = (int)HttpStatusCode.BadRequest; // 400
                    message = vex.Message;
                    break;

                case UnauthorizedException:
                    statusCode = (int)HttpStatusCode.Unauthorized; // 401
                    message = ex.Message;
                    break;

                case ForbiddenException:
                    statusCode = (int)HttpStatusCode.Forbidden; // 403
                    message = ex.Message;
                    break;

                case OperationFailedException:
                    statusCode = (int)HttpStatusCode.InternalServerError; // 500
                    message = ex.Message;
                    break;

                // add more custom cases as needed

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError; // 500
                    message = _env.IsDevelopment() ? ex.Message : "Internal Server Error";
                    details = _env.IsDevelopment() ? ex.StackTrace : null;
                    break;
            }

            _logger.LogError(ex, "An error occurred: {Message} (StatusCode: {StatusCode})", message, statusCode);

            context.Response.StatusCode = statusCode;

            var response = new ApiExceptionResponse(statusCode, message, details);

            var jsonResponse = JsonSerializer.Serialize(response, _jsonOptions);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
