namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface IAnalyticsContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
