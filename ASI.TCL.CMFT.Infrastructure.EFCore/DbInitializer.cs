//using ASI.TCL.CMFT.Application;
using ASI.TCL.CMFT.Domain.SYS;
using Microsoft.EntityFrameworkCore;

//namespace ASI.TCL.CMFT.Infrastructure.EFCore
//{
//    public class DbInitializer : IApplicationInitializer
//    {
//        private readonly ApplicationDbContext _dbContext;
//        private readonly IHashingService _hashingService;

//        public DbInitializer(ApplicationDbContext dbContext, IHashingService hashingService)
//        {
//            _dbContext = dbContext;
//            _hashingService = hashingService;
//        }

//        /// <summary>
//        /// 確保DB有預設的Role跟User
//        /// </summary>
//        /// <returns></returns>
//        public async Task InitializeAsync()
//        {
//            // 檢查是否已有 SuperAdmin 角色
//            var superRole = await _dbContext.Roles
//                .FirstOrDefaultAsync(r => r.Id == Role.SuperAdminRoleId);
//            if (superRole == null)
//            {
//                superRole = new Role(
//                    Role.SuperAdminRoleId,
//                    Role.SuperAdminRoleName,
//                    Authority.AuthList);
//                _dbContext.Roles.Add(superRole);
//            }

//            // 檢查是否已有 SuperAdmin 使用者
//            var superUser = await _dbContext.Users
//                .FirstOrDefaultAsync(u => u.Id == User.SuperAdminUserId);
//            if (superUser == null)
//            {
//                var passwordHash = _hashingService.Hash("superadmin"); // 預設密碼

//                superUser = new User(
//                    User.SuperAdminUserId,
//                    User.SuperAdminUserName,
//                    "預設帳號",
//                    passwordHash,
//                    superRole
//                );

//                _dbContext.Users.Add(superUser);
//            }

//            await _dbContext.SaveChangesAsync();
//        }
//    }
//}
