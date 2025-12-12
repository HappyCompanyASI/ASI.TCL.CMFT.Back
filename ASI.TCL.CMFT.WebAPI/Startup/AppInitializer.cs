using ASI.TCL.CMFT.Infrastructure.EFCore;
using ASI.TCL.CMFT.Infrastructure.EFCore.Identity;

namespace ASI.TCL.CMFT.WebAPI.Startup
{
    public sealed class AppInitializer : IAppInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public AppInitializer(
            ApplicationDbContext db,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _db = db;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeAsync(IHostEnvironment env, ILogger logger)
        {
            // 讀取連線字串（你原本的邏輯）
            var databaseProvider = _configuration.GetValue<string>("DatabaseProvider");
            var connectionString = _configuration.GetConnectionString(databaseProvider);
            logger.LogInformation("Connection String = {ConnectionString}", connectionString);

            // 檢查 DB 連線（建議用 async）
            var canConnect = await _db.Database.CanConnectAsync();
            if (canConnect)
                logger.LogInformation("資料庫連線成功。");
            else
                logger.LogError("資料庫無法連線（Database.CanConnectAsync() 回傳 false）。");

            // Dev 環境才 Seed（你原本的行為保留）
            if (env.IsDevelopment())
            {
                try
                {
                    var seeder = _serviceProvider.GetRequiredService<IIdentitySeeder>();
                    await seeder.SeedAsync();
                    logger.LogInformation("開發環境 Identity Seed 完成。");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "開發環境初始化預設角色與使用者失敗，但 API 仍會啟動。");
                }
            }

            logger.LogInformation("應用程式初始化完成，準備啟動。Environment={Env}", env.EnvironmentName);
        }
    }
}