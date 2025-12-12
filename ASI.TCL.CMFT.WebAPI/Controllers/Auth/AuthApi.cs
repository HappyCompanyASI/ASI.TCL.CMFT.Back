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
        /// <summary>
        /// 使用者登入驗證。
        /// </summary>
        /// <remarks>
        /// 此 API 會驗證使用者帳號與密碼是否正確，驗證成功後回傳 JWT Token，供後續受保護 API 進行身分識別與授權使用
        /// </remarks>
        /// <param name="request">
        /// 登入請求資料，Request Body: <see cref="Commands.LoginCommand"/> 。
        /// </param>
        /// <returns>
        /// 驗證成功時回傳 JWT Token 字串；驗證失敗時回傳對應的錯誤狀態碼。
        /// </returns>
        [AllowAnonymous]
        [HttpPost("login", Order = 1)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
            return await Task.FromResult(new OkResult());
        }
    }
}