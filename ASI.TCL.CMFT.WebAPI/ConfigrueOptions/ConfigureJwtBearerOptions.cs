using System.Security.Claims;
using ASI.TCL.CMFT.Application;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ASI.TCL.CMFT.WebAPI.ConfigrueOptions
{
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<ConfigureJwtBearerOptions> _logger;
        
        public ConfigureJwtBearerOptions(IServiceProvider serviceProvider, IHostEnvironment hostEnvironment, ILogger<ConfigureJwtBearerOptions> logger)
        {
            _serviceProvider = serviceProvider;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }
        public void Configure(JwtBearerOptions options)
        {
            Configure(JwtBearerDefaults.AuthenticationScheme, options);
        }
        public void Configure(string name, JwtBearerOptions options)
        {
            using var scope = _serviceProvider.CreateScope();
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
            var tokenValidationParameters = tokenService.TokenValidationParameters;
            //options.SaveToken = false;
            
            options.SaveToken = true; //  設為 true，讓 HttpContext.User 保持 JWT 身份資訊
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = tokenValidationParameters;
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = OnMessageReceived,
                OnAuthenticationFailed = OnAuthenticationFailed,
                OnTokenValidated = OnTokenValidated,
                OnChallenge = OnChallenge,
            };
        }
        
        private Task OnMessageReceived(MessageReceivedContext context)
        {
            // 從 Authorization Header 讀取 JWT
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Fail("未提供 JWT Token");//OnAuthenticationFailed 會被跳過。
                return Task.CompletedTask;
            }
            context.Token = token;

            return Task.CompletedTask;
        }
        //JWT 驗證失敗時執行
        private Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.LogError("JWT 驗證失敗: {Exception}", context.Exception);
            return Task.CompletedTask;
        }
        //JWT 驗證成功後執行
        private async Task OnTokenValidated(TokenValidatedContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();

            var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                context.Fail("無法取得使用者 ID");
                return;
            }
            var isUserExist = await identityService.IsUserExistAsync(userId);

            if (!isUserExist)
            {
                context.Fail("此帳戶已不存在");
                return;
            }

            _logger.LogInformation($"Token 驗證成功，使用者 ID：{userId}");
        }
        //身份驗證失敗（OnAuthenticationFailed / OnTokenValidated.Fail()）時執行。
        private Task OnChallenge(JwtBearerChallengeContext context)
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync("{\"message\": \"未授權，請提供有效的 JWT Token\"}");
        }
    }
}