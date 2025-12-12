using System.Security.Claims;
using ASI.TCL.CMFT.Application;

namespace ASI.TCL.CMFT.WebAPI
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public Guid GetCurrentUserId()
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null)
                return Guid.Empty;

            var user = httpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
                return Guid.Empty;

            // 從 JWT / Cookie 的 Claims 讀取 NameIdentifier
            var idValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(idValue))
                return Guid.Empty;

            return Guid.TryParse(idValue, out var userId) ? userId : Guid.Empty;
        }
    }
}
