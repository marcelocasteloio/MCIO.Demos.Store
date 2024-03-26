using MCIO.Demos.Store.BuildingBlock.Resilience;
using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions.Models;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Analytics.Interfaces;

namespace MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Analytics;

public class AnalyticsPingHttpOperationResiliencePolicy
    : ResiliencePolicyBase,
    IAnalyticsPingHttpOperationResiliencePolicy
{
    // Fields
    private readonly Config.Config _config;

    // Constructors
    public AnalyticsPingHttpOperationResiliencePolicy(Config.Config config)
    {
        _config = config;
    }

    // Protected Methods
    protected override void ConfigureInternal(ResiliencePolicyOptions options)
    {
        options
            .WithCustomIdentificationOptions(
                name: nameof(AnalyticsPingHttpOperationResiliencePolicy)
            )
            .WithCustomRetryOptions(
                retryMaxAttemptCount: 3,
                retryAttemptWaitingTimeFunction: attempt => TimeSpan.FromSeconds(2 ^ (attempt - 1))
            )
            .WithCustomCircuitBreakerOptions(
                circuitBreakerWaitingTimeFunction: () => TimeSpan.FromSeconds(30)
            );
    }
}
