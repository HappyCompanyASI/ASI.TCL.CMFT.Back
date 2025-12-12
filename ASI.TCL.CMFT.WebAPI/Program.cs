using ASI.TCL.CMFT.WebAPI.Extensions;

namespace ASI.TCL.CMFT.WebAPI
{
    public static class Program
    {
        /// <summary>
        /// 應用程式進入點 (Entry Point)
        /// 
        /// - 使用 async Main，支援非同步啟動流程
        /// - 不在這裡阻塞等待初始化（DB / Seed）
        /// - 初始化改由 Background HostedService 執行，確保 Swagger / HTTP 能立即啟動
        /// </summary>
        public static async Task Main(string[] args)
        {
            // 建立 WebApplicationBuilder
            // 這一步會初始化：
            // - Configuration（appsettings.json、環境變數等）
            // - Logging
            // - DI Container
            var builder = WebApplication.CreateBuilder(args);

            // 註冊所有應用程式所需的服務
            // 包含：
            // - Application / Infrastructure / Identity
            // - DbContext
            // - Swagger
            // - Middleware 相關服務
            builder.Services.AddServices(builder.Configuration);

            // 註冊背景初始化服務（HostedService）
            // 目的：
            // - 將「DB 連線檢查 / Identity Seed」移出啟動流程
            // - 不阻塞 HTTP Server 啟動
            // - 確保 Swagger 能立即開啟
            //
            // 此服務會在應用程式啟動後，以背景方式執行一次初始化邏輯
            builder.Services.AddHostedService<AppInitializationHostedService>();

            // 建立 WebApplication
            // 到這一步為止：
            // - DI Container 已完成
            // - 但 HTTP Server 尚未開始監聽
            var app = builder.Build();

            // 設定 HTTP Middleware Pipeline
            // 例如：
            // - Exception Handling
            // - Swagger / SwaggerUI
            // - Authentication / Authorization
            // - Routing
            //
            // 注意：
            // Middleware 設定完成後，實際上仍未開始對外提供服務
            app.UseMiddlewares();

            // 啟動 HTTP Server（Kestrel）
            // - 開始監聽請求
            // - Swagger 此時即可被存取
            // - Background HostedService 會同時在背景執行初始化
            await app.RunAsync();
        }
    }

}