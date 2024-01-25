using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck;

namespace MCIO.Demos.Store.Pricing.WebApi.HealthCheck;

public class Liveness
    : HealthCheckBase
{
    // Constants
    public const string NOT_READY = nameof(NOT_READY);

    // Protected Methods
    protected override Task CheckHealthInternalAsync(Dictionary<string, object> serviceStatusDictionary)
    {
        return Task.CompletedTask;
    }
}
