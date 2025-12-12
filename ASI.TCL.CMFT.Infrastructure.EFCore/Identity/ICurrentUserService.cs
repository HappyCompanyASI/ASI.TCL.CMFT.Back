namespace ASI.TCL.CMFT.Infrastructure.EFCore.Identity
{
    public interface ICurrentUserService
    {
        Guid GetCurrentUserId();
    }
}