namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface ICatalogContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
