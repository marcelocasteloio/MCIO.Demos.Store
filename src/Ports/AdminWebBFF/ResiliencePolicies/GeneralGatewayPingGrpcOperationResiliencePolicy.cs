using MCIO.Demos.Store.BuildingBlock.Resilience;
using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions.Models;
using MCIO.Demos.Store.Ports.AdminWebBFF.ResiliencePolicies.Interfaces;

namespace MCIO.Demos.Store.Ports.AdminWebBFF.ResiliencePolicies;

public class GeneralGatewayPingGrpcOperationResiliencePolicy
    : ResiliencePolicyBase,
    IGeneralGatewayPingGrpcOperationResiliencePolicy
{
    protected override void ConfigureInternal(ResiliencePolicyOptions options)
    {
        options
            .WithCustomIdentificationOptions(
                name: nameof(GeneralGatewayPingGrpcOperationResiliencePolicy)
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
