using System.Security.Claims;
using ASI.TCL.CMFT.Messages.SYS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.Identity;

public static class IdentitySeedData
{
    public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();

        //--------------------------------------------------
        // 1. 確保角色存在 + 權限 Claims
        //--------------------------------------------------
        // Member
        var memberRole = await EnsureRoleWithPermissionsAsync(
            roleManager,
            "Member",
            PermissionKey.GetMemberPermissions(),
            belongUnit: "System",
            description: "一般會員角色"
        );

        // Administrator
        var adminRole = await EnsureRoleWithPermissionsAsync(
            roleManager,
            "Administrator",
            PermissionKey.GetAdminstratorPermissions(),
            belongUnit: "System",
            description: "系統管理員角色"
        );

        //--------------------------------------------------
        // 2. 確保預設管理員帳號存在
        //--------------------------------------------------
        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = "admin",   // 也就是你的 Account
                FirstName = "System",
                LastName = "Administrator",
                IsActive = true
            };

            var createResult = await userManager.CreateAsync(adminUser, "admin"); // 你密碼規則允許 5 碼小寫
            if (!createResult.Succeeded)
            {
                // 這裡你要嘛丟 Exception，要嘛記 Log，隨你
                throw new Exception("建立預設管理員帳號失敗: " +
                                    string.Join("; ", createResult.Errors.Select(e => e.Description)));
            }

            // 先清掉舊 claims（理論上新建是沒有，不過保險起見）
            var claims = await userManager.GetClaimsAsync(adminUser);
            if (claims.Any())
            {
                await userManager.RemoveClaimsAsync(adminUser, claims);
            }

            await userManager.AddClaimsAsync(adminUser, new List<Claim>
            {
                new("FirstName", "System"),
                new("LastName",  "Administrator")
            });
        }

        //--------------------------------------------------
        // 3. 確保管理員帳號有 Administrator 角色
        //--------------------------------------------------
        var inRole = await userManager.IsInRoleAsync(adminUser, "Administrator");
        if (!inRole)
        {
            var addToRoleResult = await userManager.AddToRoleAsync(adminUser, "Administrator");
            if (!addToRoleResult.Succeeded)
            {
                throw new Exception("將 admin 加入 Administrator 角色失敗: " +
                                    string.Join("; ", addToRoleResult.Errors.Select(e => e.Description)));
            }
        }
    }

    /// <summary>
    /// 確保角色存在，並套用指定的 Permission claims。
    /// </summary>
    private static async Task<AppRole> EnsureRoleWithPermissionsAsync(
        RoleManager<AppRole> roleManager,
        string roleName,
        IEnumerable<string> permissions,
        string belongUnit,
        string description)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new AppRole
            {
                Name = roleName,
                //BelongUnit = belongUnit,
                Description = description,
                IsActive = true
            };

            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception($"建立角色 {roleName} 失敗: " +
                                    string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }

        var roleClaims = await roleManager.GetClaimsAsync(role);
        foreach (var permission in permissions)
        {
            var permissionClaim = new Claim("Permission", permission);
            if (!roleClaims.Any(c => c.Type == permissionClaim.Type && c.Value == permissionClaim.Value))
            {
                //更新「權限表」
                await roleManager.AddClaimAsync(role, permissionClaim);
            }
        }

        return role;
    }
}