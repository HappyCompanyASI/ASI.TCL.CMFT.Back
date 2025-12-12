using ASI.TCL.CMFT.WebAPI.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASI.TCL.CMFT.WebAPI.Controllers.SYS
{
    [ApiController]
    [Route("api/test")]
    [SwaggerGroup(SwaggerGroupKind.Test)]
    public class TestApi : ControllerBase
    {
        private readonly ILogger<TestApi> _logger;

        public TestApi( ILogger<TestApi> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("handledexception")]
        public IActionResult HandledExceptionPost()
            => throw new UnauthorizedAccessException();

        [AllowAnonymous]
        [HttpPost("unhandledexception")]
        public IActionResult UnhandledExceptionPost() 
            => throw new IndexOutOfRangeException();
    }
}