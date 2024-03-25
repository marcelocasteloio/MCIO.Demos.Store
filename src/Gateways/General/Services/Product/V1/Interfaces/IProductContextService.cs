namespace MCIO.Demos.Store.Gateways.General.Services.Product.V1.Interfaces;

public interface IProductContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
