using ASI.TCL.CMFT.WebAPI.Startup;

namespace ASI.TCL.CMFT.WebAPI
{
    public sealed class AppInitializationHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostEnvironment _env;
        private readonly ILogger<AppInitializationHostedService> _logger;

        public AppInitializationHostedService(
            IServiceProvider serviceProvider,
            IHostEnvironment env,
            ILogger<AppInitializationHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _env = env;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 讓 HTTP 先起來（Swagger 先能打）
            await Task.Yield();
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var sp = scope.ServiceProvider;

                var initializer = sp.GetRequiredService<IAppInitializer>();

                _logger.LogInformation("背景初始化開始。Environment={Env}", _env.EnvironmentName);

                await initializer.InitializeAsync(_env, _logger);

                _logger.LogInformation("背景初始化完成。");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "背景初始化失敗，但 API 仍會繼續提供服務。");
            }
        }
    }
}