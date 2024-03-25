namespace MCIO.Demos.Store.Gateways.General.Services.Order.V1.Interfaces;

public interface IOrderContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
