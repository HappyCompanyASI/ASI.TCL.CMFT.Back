using ASI.TCL.CMFT.Application;
using ASI.TCL.CMFT.Messages.Auth;
using ASI.TCL.CMFT.WebAPI.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASI.TCL.CMFT.WebAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    [SwaggerGroup(SwaggerGroupKind.Auth)]
    public class AuthApi(IIdentityService identityService, ITokenService tokenService, ILogger<AuthApi> logger)
        : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login", Order = 1)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] Commands.LoginCommand request)
        {
            logger.LogInformation($"登入帳號{request.Account}; 密碼{request.Password}");
            var claims = await identityService.AuthenticateUserAsync(request.Account, request.Password);
            var token = tokenService.GeneratorToken(claims);

            return new OkObjectResult(token);
        }

        [AllowAnonymous]
        [HttpPost("logout", Order = 2)]
        public async Task<IActionResult> Post([FromBody] Commands.LogoutCommand request)
        {
            //await identityService.CreateUserAsync(request.Account, request.Password, request.FirstName, request.LastName);
            return new OkResult();
        }
    }
}