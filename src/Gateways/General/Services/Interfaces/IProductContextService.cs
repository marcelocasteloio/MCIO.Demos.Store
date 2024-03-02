namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface IProductContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
