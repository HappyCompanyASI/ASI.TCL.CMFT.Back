using System.Security.Claims;
using ASI.TCL.CMFT.Application;
using Microsoft.AspNetCore.Identity;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.Identity
{
    public class IdentityService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        : IIdentityService
    {
        // ----------------------------------------------------------
        // 角色管理
        // ----------------------------------------------------------
        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                return false;

            var role = new AppRole
            {
                Name = roleName
            };

            var result = await roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public async Task<IDomainRole> GetRoleByNameAsync(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            // AppRole 已經實作 IAppRole，可以直接回傳
            return role;
        }

        public Task<IList<Claim>> GetRoleClaimsAsync(string rolName)
        {
            throw new NotImplementedException();
        }

        // ----------------------------------------------------------
        // 使用者管理
        // ----------------------------------------------------------
        public async Task<bool> CreateUserAsync(string account, string password, string firstName, string lastName)
        {
            // 用 AppUser，不要再 new IdentityUser
            var user = new AppUser
            {
                UserName = account,
                Account = account
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("註冊失敗");

            await userManager.AddClaimAsync(user, new Claim("FirstName", firstName));
            await userManager.AddClaimAsync(user, new Claim("LastName", lastName));

            result = await userManager.AddToRoleAsync(user, "Member");
            if (result.Succeeded)
                return true;

            await userManager.DeleteAsync(user);
            return false;
        }

        public async Task<List<Claim>> AuthenticateUserAsync(string userAccount, string password)
        {
            var user = await userManager.FindByNameAsync(userAccount);
            if (user == null)
                throw new KeyNotFoundException("使用者不存在。");

            var passwordCheck = await userManager.CheckPasswordAsync(user, password);
            if (!passwordCheck)
                throw new InvalidOperationException("登入失敗，密碼錯誤。");

            if (await userManager.IsLockedOutAsync(user))
                throw new InvalidOperationException("帳號已被鎖定，請稍後再試。");

            var claims = new List<Claim>();

            var userId = user.Id;
            var userName = user.UserName ?? string.Empty;

            // 加入 UserId 和 UserName 到 Claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, userName));

            // 取得 User Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // 取得 User Roles 並轉換成 Claims
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var roleName in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));

                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims);
                }
            }

            return claims;
        }

        public async Task<IList<Claim>> GetUserClaimsAsync(string account)
        {
            var user = await userManager.FindByNameAsync(account);
            if (user == null)
                return null;

            var userClaims = await userManager.GetClaimsAsync(user);
            return userClaims;
        }

        public async Task<IDomainUser> GetUserByAccountAsync(string account)
        {
            var user = await userManager.FindByNameAsync(account);
            if (user == null)
                return null;

            // 直接回傳 AppUser（IdentityUser 已經是 AppUser）
            return user;
            // 如果你只想回簡化版，也可以這樣：
            // return new AppUser { Id = user.Id, Account = user.UserName };
        }

        public async Task<IDomainRole> GetAccountRoleAsync(string account)
        {
            var user = await userManager.FindByNameAsync(account);
            if (user == null)
                return null;

            var roleList = await userManager.GetRolesAsync(user);
            var roleName = roleList.FirstOrDefault();

            if (string.IsNullOrEmpty(roleName))
                return null;

            var role = await roleManager.FindByNameAsync(roleName);
            return role;
        }

        public async Task<bool> ChangePasswordAsync(string account, string oldPassword, string newPassword)
        {
            var user = await userManager.FindByNameAsync(account);
            if (user == null)
                return false;

            var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> AssignRoleAsync(string account, string roleName)
        {
            var user = await userManager.FindByNameAsync(account);
            if (user == null)
                return false;

            if (!await roleManager.RoleExistsAsync(roleName))
                return false;

            var result = await userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> IsUserExistAsync(string userID)
        {
            var user = await userManager.FindByIdAsync(userID);
            return user != null;
        }
    }
}
