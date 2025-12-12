using Microsoft.AspNetCore.Mvc;

namespace ASI.TCL.CMFT.WebAPI.RequestPipeline
{
    public static class RequestHandler
    {
        public static async Task<IActionResult> HandleCommand<TCommand>(
            TCommand request,
            Func<TCommand, Task> handler,
            ILogger logger)
        {
            logger.LogDebug($"Handling HTTP request of type {typeof(TCommand).Name}");
            await handler(request);
            return new OkResult();
        }

        public static async Task<IActionResult> HandleQuery<TQueryModel>(
            Func<Task<TQueryModel>> query,
            ILogger logger)
        {
            logger.LogDebug($"Handling HTTP request of type {typeof(TQueryModel).Name}");
            var result = await query();
            return new OkObjectResult(result);
        }
    }
}
