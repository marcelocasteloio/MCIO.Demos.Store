namespace MCIO.Demos.Store.Notification.WebApi.Services;

public class StartupService
{
    // Properties
    public static bool HasStarted { get; private set; }

    // Constructors
    static StartupService()
    {
        HasStarted = false;
    }
    protected StartupService() { }

    // Public Methods
    public static Task TryStartupAsync(
        IServiceProvider serviceProvider
    )
    {
        HasStarted = true;

        return Task.CompletedTask;
    }
}

