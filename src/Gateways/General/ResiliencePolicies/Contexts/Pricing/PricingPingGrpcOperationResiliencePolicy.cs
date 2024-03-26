using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions.Models;
using MCIO.Demos.Store.BuildingBlock.Resilience;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Pricing.Interfaces;

namespace MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Pricing;

public class PricingPingGrpcOperationResiliencePolicy
    : ResiliencePolicyBase,
    IPricingPingGrpcOperationResiliencePolicy
{
    protected override void ConfigureInternal(ResiliencePolicyOptions options)
    {
        options
            .WithCustomIdentificationOptions(
                name: nameof(PricingPingGrpcOperationResiliencePolicy)
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