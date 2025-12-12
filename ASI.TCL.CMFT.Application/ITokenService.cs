using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ASI.TCL.CMFT.Application
{
    public interface ITokenService
    {
        TokenValidationParameters TokenValidationParameters { get; }
        string GeneratorToken(List<Claim> claims);
        bool ValidateToken(string token);
    }
}