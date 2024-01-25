using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck.Models.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck;
public abstract class HealthCheckBase
    : IHealthCheck
{
    // Public Methods
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var serviceStatusDictionary = new Dictionary<string, object>();

        await CheckHealthInternalAsync(serviceStatusDictionary);

        var isHealthy = !serviceStatusDictionary.Any(q => (ServiceStatus)q.Value == ServiceStatus.Unhealthy);

        return isHealthy
            ? HealthCheckResult.Healthy(data: serviceStatusDictionary)
            : new HealthCheckResult(status: context.Registration.FailureStatus, data: serviceStatusDictionary);
    }

    // Protected Methods
    protected abstract Task CheckHealthInternalAsync(Dictionary<string, object> serviceStatusDictionary);
}
