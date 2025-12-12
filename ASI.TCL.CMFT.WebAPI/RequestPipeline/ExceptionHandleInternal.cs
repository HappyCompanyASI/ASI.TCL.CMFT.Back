using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace ASI.TCL.CMFT.WebAPI.RequestPipeline
{
    internal class ExceptionHandleInternal
    {
        public static async Task Handle<T>(HttpContext httpContext, Exception exception, CancellationToken cancellationToken, ILogger<T> _logger)
        {
            var (title,statusCode) = exception switch
            {
                //400
                InvalidOperationException => ("錯誤的請求", StatusCodes.Status400BadRequest),
                //401
                UnauthorizedAccessException => ("未授權的請求", StatusCodes.Status401Unauthorized),
                //404
                KeyNotFoundException => ("資源未找到",StatusCodes.Status404NotFound),
                //500
                _ => ("伺服器內部發生錯誤",StatusCodes.Status500InternalServerError)
            };
 
           switch (statusCode)
            {
                case >= 400 and < 500:
                    _logger.LogWarning("Message={message}", exception.Message);
                    break;
                case >= 500:
                    _logger.LogError(exception, "Message={message}", exception.Message);
                    break;
            }
            //------------------------------------------------------
            // Response Header
            //------------------------------------------------------
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.Headers["X-Correlation-Id"] = httpContext.TraceIdentifier;

            //============================
            //Response Body
            //============================
            var activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
            var problemDetails = new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Path}"
            };
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }
    }
}