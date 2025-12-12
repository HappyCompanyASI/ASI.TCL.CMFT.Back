namespace ASI.TCL.CMFT.WebAPI.RequestPipeline
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, CancellationToken cancellationToken)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await ExceptionHandleInternal.Handle(httpContext, exception, cancellationToken, _logger);
            }
        }
    }

}