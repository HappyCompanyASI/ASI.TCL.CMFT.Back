using Microsoft.AspNetCore.Diagnostics;

namespace ASI.TCL.CMFT.WebAPI.RequestPipeline
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            await ExceptionHandleInternal.Handle(httpContext, exception, cancellationToken, _logger);

            return true;
        }
    }
}