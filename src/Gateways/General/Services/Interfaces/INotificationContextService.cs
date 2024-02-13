namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface INotificationContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
