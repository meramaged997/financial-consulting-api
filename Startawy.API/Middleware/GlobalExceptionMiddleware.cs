using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Startawy.Application.Common;
        
namespace Startawy.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next   = next;
        _logger = logger;
        _env    = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message, userErrors) = exception switch
        {
            ArgumentNullException ane => ((int)HttpStatusCode.BadRequest, "A required value was not provided.", new List<string> { ane.ParamName ?? ane.Message }),
            ArgumentException aex     => ((int)HttpStatusCode.BadRequest, aex.Message, null),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "You are not authorized to perform this action.", null),
            KeyNotFoundException kex  => ((int)HttpStatusCode.NotFound, "The requested resource was not found.", null),
            _                         => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.", null)
        };

        context.Response.StatusCode = statusCode;

        var response = ApiResponse<object>.Fail(message, userErrors);

        if (_env.IsDevelopment())
        {
            var devInfo = new
            {
                TraceId = context.TraceIdentifier,
                Exception = new
                {
                    Type = exception.GetType().FullName,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace
                }
            };

            var wrapper = new
            {
                response.Success,
                response.Message,
                response.Data,
                response.Errors,
                Dev = devInfo
            };

            var devJson = JsonSerializer.Serialize(wrapper, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            });

            await context.Response.WriteAsync(devJson);
            return;
        }

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        await context.Response.WriteAsync(json);
    }
}
