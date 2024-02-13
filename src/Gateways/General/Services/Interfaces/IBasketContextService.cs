namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface IBasketContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
