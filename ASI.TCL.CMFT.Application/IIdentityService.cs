using System.Security.Claims;

namespace ASI.TCL.CMFT.Application
{
    public interface IIdentityService
    {
        // 角色管理
        Task<bool> CreateRoleAsync(string roleName);
        Task<IDomainRole> GetRoleByNameAsync(string roleName);
        Task<IList<Claim>> GetRoleClaimsAsync(string rolName);
        // 使用者管理
        Task<List<Claim>> AuthenticateUserAsync(string userAccount, string password);
        Task<bool> CreateUserAsync(string account, string password, string firstName, string lastName);
        Task<IList<Claim>> GetUserClaimsAsync(string account);
        Task<IDomainUser> GetUserByAccountAsync(string account);
        Task<IDomainRole> GetAccountRoleAsync(string account);
        Task<bool> ChangePasswordAsync(string account, string oldPassword, string newPassword);
        Task<bool> AssignRoleAsync(string account, string roleName);
        Task<bool> IsUserExistAsync(string userID);
    }
}