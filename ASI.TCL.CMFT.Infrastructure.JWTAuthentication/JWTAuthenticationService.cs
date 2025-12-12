using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ASI.TCL.CMFT.Application;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ASI.TCL.CMFT.Infrastructure.JWTAuthentication
{
    public class JWTAuthenticationService : ITokenService
    {
        public TokenValidationParameters TokenValidationParameters => tokenParams;

        private readonly JWTSettings _jwtSettings;
        private readonly TokenValidationParameters tokenParams;
        private readonly ILogger<JWTAuthenticationService> _logger;

        public JWTAuthenticationService(IOptions<JWTSettings> jwtSettings, ILogger<JWTAuthenticationService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;

            tokenParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,

                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret!)),

                RoleClaimType = "role",
            };
        }
        public string GeneratorToken(List<Claim> claims)
        {
            var secret = _jwtSettings.Secret;
            var validIssuer = _jwtSettings.Issuer;
            var validAudience = _jwtSettings.Audience;
            var expirationMinutes = _jwtSettings.ExpirationMinutes;
            var key = Encoding.UTF8.GetBytes(secret!);

            //因為驗證時有設定 RequireExpirationTime = true
            //這樣產生的 Token 內會自動包含 exp Claim，前端可以解析 exp 知道過期時間！
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes); // 設定 Token 過期時間

            // 設定 Token 描述
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt, // Token 過期時間
                Issuer = validIssuer,
                Audience = validAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            // 產生 JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, tokenParams, out _);
            return true;
        }
    }
}