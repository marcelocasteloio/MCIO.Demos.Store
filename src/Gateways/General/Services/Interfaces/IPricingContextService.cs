namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface IPricingContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
