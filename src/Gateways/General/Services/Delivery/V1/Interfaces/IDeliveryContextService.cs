namespace MCIO.Demos.Store.Gateways.General.Services.Delivery.V1.Interfaces;

public interface IDeliveryContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
