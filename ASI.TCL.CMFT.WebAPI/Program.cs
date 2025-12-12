using ASI.TCL.CMFT.Infrastructure.EFCore;
using ASI.TCL.CMFT.WebAPI.Extensions;

namespace ASI.TCL.CMFT.WebAPI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 服務註冊
            builder.Services.AddServices(builder.Configuration);

            var app = builder.Build();
            var logger = app.Logger;

            // 啟動前：檢查 DB 連線 +（Dev 環境）嘗試 Seed
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // 讀取連線字串
                    var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider");
                    var connectionString = builder.Configuration.GetConnectionString(databaseProvider);
                    logger.LogInformation("Connection String = {ConnectionString}", connectionString);

                    // 嘗試建立 DbContext 並檢查連線
                    var db = services.GetRequiredService<ApplicationDbContext>();
                    if (db.Database.CanConnect())
                    {
                        logger.LogInformation("資料庫連線成功。");
                    }
                    else
                    {
                        logger.LogError("資料庫無法連線（Database.CanConnect() 回傳 false）。");
                    }

                    // 只在 Development 做 Seed，失敗也不會讓程式掛掉
                    //if (app.Environment.IsDevelopment())
                    //{
                    //    try
                    //    {
                    //        IdentitySeedData
                    //            .SeedRolesAndUsersAsync(services)
                    //            .GetAwaiter()
                    //            .GetResult();

                    //        logger.LogInformation("開發環境 SeedRolesAndUsers 完成。");
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        logger.LogError(ex,
                    //            "開發環境初始化預設角色與使用者失敗，但 API 仍會啟動。");
                    //    }
                    //}

                    logger.LogInformation(
                        "應用程式初始化完成，準備啟動。Environment={Env}",
                        app.Environment.EnvironmentName);
                }
                catch (Exception ex)
                {
                    // 這裡是「連 DbContext 都抓不到」或其他啟動期例外
                    logger.LogError(ex,
                        "啟動時檢查資料庫或執行初始化時發生未預期錯誤，API 仍會啟動。");
                }
            }

            // 中介層
            app.UseMiddlewares();

            app.Run();
        }
    }

}