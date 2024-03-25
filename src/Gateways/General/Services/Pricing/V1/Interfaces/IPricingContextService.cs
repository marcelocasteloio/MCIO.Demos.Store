namespace MCIO.Demos.Store.Gateways.General.Services.Pricing.V1.Interfaces;

public interface IPricingContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
