namespace MCIO.Demos.Store.Gateways.General.Services.Catalog.V1.Interfaces;

public interface ICatalogContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
