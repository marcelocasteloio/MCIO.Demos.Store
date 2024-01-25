using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck;
using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck.Models.Enums;
using MCIO.Demos.Store.Customer.WebApi.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MCIO.Demos.Store.Customer.WebApi.HealthCheck;

public class Readiness
    : HealthCheckBase
{
    // Constants
    public const string NOT_READY = nameof(NOT_READY);

    // Protected Methods
    protected override Task CheckHealthInternalAsync(Dictionary<string, object> serviceStatusDictionary)
    {
        return Task.FromResult(
            StartupService.HasStarted
                ? HealthCheckResult.Healthy()
                : new HealthCheckResult(
                    status: HealthStatus.Unhealthy,
                    data: new Dictionary<string, object>() { { NOT_READY, ServiceStatus.Unhealthy } }
                )
        );
    }
}
