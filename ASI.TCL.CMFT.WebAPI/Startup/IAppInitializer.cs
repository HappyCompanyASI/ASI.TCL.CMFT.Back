namespace ASI.TCL.CMFT.WebAPI.Startup
{
    public interface IAppInitializer
    {
        Task InitializeAsync(IHostEnvironment env, ILogger logger);
    }
}