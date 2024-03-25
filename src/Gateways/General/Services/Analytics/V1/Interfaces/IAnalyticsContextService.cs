namespace MCIO.Demos.Store.Gateways.General.Services.Analytics.V1.Interfaces;

public interface IAnalyticsContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
