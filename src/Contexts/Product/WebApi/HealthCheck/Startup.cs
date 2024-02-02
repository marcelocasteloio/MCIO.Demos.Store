using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck;

namespace MCIO.Demos.Store.Product.WebApi.HealthCheck;

public class Startup
    : HealthCheckBase
{
    protected override Task CheckHealthInternalAsync(Dictionary<string, object> serviceStatusDictionary)
    {
        return Task.CompletedTask;
    }
}
