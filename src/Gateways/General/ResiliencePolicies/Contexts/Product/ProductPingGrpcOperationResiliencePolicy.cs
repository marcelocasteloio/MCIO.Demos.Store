using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions.Models;
using MCIO.Demos.Store.BuildingBlock.Resilience;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Product.Interfaces;

namespace MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Product;

public class ProductPingGrpcOperationResiliencePolicy
    : ResiliencePolicyBase,
    IProductPingGrpcOperationResiliencePolicy
{
    protected override void ConfigureInternal(ResiliencePolicyOptions options)
    {
        options
            .WithCustomIdentificationOptions(
                name: nameof(ProductPingGrpcOperationResiliencePolicy)
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