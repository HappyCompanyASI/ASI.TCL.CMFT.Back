using ASI.TCL.CMFT.WebAPI.Startup;

namespace ASI.TCL.CMFT.WebAPI.Extensions
{
    public static class ApplicationInitializationExtensions
    {
        public static async Task InitializeAsync(this WebApplication app)
        {
            var logger = app.Logger;

            using var scope = app.Services.CreateScope();
            var sp = scope.ServiceProvider;

            try
            {
                var initializer = sp.GetRequiredService<IAppInitializer>();
                await initializer.InitializeAsync(app.Environment, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "啟動初始化失敗（DB 檢查 / Seed），但 API 仍會啟動。");
            }
        }
    }
}