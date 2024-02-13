namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface IOrderContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
