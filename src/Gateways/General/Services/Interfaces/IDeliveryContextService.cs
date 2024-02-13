namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface IDeliveryContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
