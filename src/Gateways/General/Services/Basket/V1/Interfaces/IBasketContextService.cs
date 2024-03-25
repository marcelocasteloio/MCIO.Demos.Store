namespace MCIO.Demos.Store.Gateways.General.Services.Basket.V1.Interfaces;

public interface IBasketContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
