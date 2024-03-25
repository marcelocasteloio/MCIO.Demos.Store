namespace MCIO.Demos.Store.Gateways.General.Services.Notification.V1.Interfaces;

public interface INotificationContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
