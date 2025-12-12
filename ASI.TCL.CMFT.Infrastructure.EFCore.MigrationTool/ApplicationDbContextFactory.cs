using ASI.TCL.CMFT.Infrastructure.EFCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ASI.TCL.CMFT.Infrastructure.EFCore.MigrationTool
{
    public class MigrationCurrentUserService : ICurrentUserService
    {
        public Guid GetCurrentUserId()
        {
            // Migration 執行時沒有登入使用者，回傳空字串即可
            return Guid.Empty;
        }
    }
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private readonly string _projectDirectory;

        public ApplicationDbContextFactory()
        {
            var projectDirectory = Tool.FindProjectDirectory(Tool.WebApiProjectName);
            _projectDirectory = projectDirectory;
        }


        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(_projectDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
                .Build();

            var databaseProvider = configuration["DatabaseProvider"];

            if (string.IsNullOrEmpty(databaseProvider))
            {
                throw new Exception("無法找到 DatabaseProvider，請檢查 appsettings.json");
            }

            var connectionString = configuration.GetConnectionString(databaseProvider);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"無法找到 {databaseProvider} 的 ConnectionString，請檢查 appsettings.json");
            }

            Console.WriteLine($"使用的 DatabaseProvider: {databaseProvider}");
            Console.WriteLine($"連線字串: {connectionString}");


            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // 使用 typeof(ApplicationDbContext).Assembly 來確保 Migrations 存放在 Infrastructure.EFCore
            optionsBuilder.UseNpgsql(connectionString, sqlOptions =>
                sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));

            return new ApplicationDbContext(optionsBuilder.Options, new MigrationCurrentUserService());
        }

    }

}
