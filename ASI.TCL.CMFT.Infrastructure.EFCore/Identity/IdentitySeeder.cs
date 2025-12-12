using System.Security.Claims;

using ASI.TCL.CMFT.Domain.SYS;
using Microsoft.AspNetCore.Identity;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.Identity
{
    public sealed class IdentitySeeder : IIdentitySeeder
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public IdentitySeeder(
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            var permissions = Authority.AuthorityList.Select(x => x.Code);

            var adminRole = await EnsureRoleWithPermissionsAsync(
                "Administrator",
                permissions,
                "系統管理員角色"
            );

            var adminUser = await EnsureAdminUserAsync();

            if (!await _userManager.IsInRoleAsync(adminUser, adminRole.Name))
            {
                await _userManager.AddToRoleAsync(adminUser, adminRole.Name);
            }
        }


        // ==================================================
        // Role
        // ==================================================
        private async Task<AppRole> EnsureRoleWithPermissionsAsync(
            string roleName,
            IEnumerable<string> permissions,
            string description)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                role = new AppRole
                {
                    Name = roleName,
                    Description = description,
                    IsActive = true
                };

                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception($"建立角色 {roleName} 失敗: " +
                                        string.Join("; ", result.Errors.Select(e => e.Description)));
                }
            }

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (!roleClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                {
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }

            return role;
        }


        // ==================================================
        // User
        // ==================================================
        private async Task<AppUser> EnsureAdminUserAsync()
        {
            const string systemUnit = "SYSTEM";

            var user = await _userManager.FindByNameAsync("admin");
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = "admin",
                    FirstName = "System",
                    LastName = "Administrator",
                    BelongUnit = systemUnit,   // ✅ 關鍵
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, "admin");
                if (!result.Succeeded)
                {
                    throw new Exception("建立 admin 使用者失敗: " +
                                        string.Join("; ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                // 保證舊資料不會殘留 null
                if (string.IsNullOrWhiteSpace(user.BelongUnit))
                {
                    user.BelongUnit = systemUnit;
                    await _userManager.UpdateAsync(user);
                }
            }

            return user;
        }

        private async Task UpsertUserClaimAsync(AppUser user, string type, string value)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var existing = claims.FirstOrDefault(x => x.Type == type);
            if (existing == null)
            {
                var addResult = await _userManager.AddClaimAsync(user, new Claim(type, value));
                if (!addResult.Succeeded)
                {
                    throw new Exception($"新增使用者 Claim 失敗({type}): " +
                                        string.Join("; ", addResult.Errors.Select(e => e.Description)));
                }
                return;
            }

            if (existing.Value != value)
            {
                var removeResult = await _userManager.RemoveClaimAsync(user, existing);
                if (!removeResult.Succeeded)
                {
                    throw new Exception($"移除使用者 Claim 失敗({type}): " +
                                        string.Join("; ", removeResult.Errors.Select(e => e.Description)));
                }

                var addResult = await _userManager.AddClaimAsync(user, new Claim(type, value));
                if (!addResult.Succeeded)
                {
                    throw new Exception($"更新使用者 Claim 失敗({type}): " +
                                        string.Join("; ", addResult.Errors.Select(e => e.Description)));
                }
            }
        }

        // ==================================================
        // System Unit Id
        // ==================================================
        private static Guid GetSystemUnitId()
        {
            // 方案 A：固定 Guid（推薦）
            // 用固定值 seed，所有環境一致；也方便你寫 FK 與 CreatedBy/UpdatedBy 的 System User
            return Guid.Parse("11111111-1111-1111-1111-111111111111");
        }
    }
}