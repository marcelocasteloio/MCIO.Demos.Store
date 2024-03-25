using MCIO.Demos.Store.BuildingBlock.Resilience;
using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions.Models;
using MCIO.Demos.Store.Ports.ClientMobileBFF.ResiliencePolicies.Interfaces;

namespace MCIO.Demos.Store.Ports.ClientMobileBFF.ResiliencePolicies;

public class GeneralGatewayPingHttpOperationResiliencePolicy
    : ResiliencePolicyBase,
    IGeneralGatewayPingHttpOperationResiliencePolicy
{
    // Fields
    private readonly Config.Config _config;

    // Constructors
    public GeneralGatewayPingHttpOperationResiliencePolicy(Config.Config config)
    {
        _config = config;
    }

    // Protected Methods
    protected override void ConfigureInternal(ResiliencePolicyOptions options)
    {
        options
            .WithCustomIdentificationOptions(
                name: nameof(GeneralGatewayPingHttpOperationResiliencePolicy)
            )
            .WithCustomRetryOptions(
                retryMaxAttemptCount: 3,
                retryAttemptWaitingTimeFunction: attempt => TimeSpan.FromSeconds(2 ^ attempt - 1)
            )
            .WithCustomCircuitBreakerOptions(
                circuitBreakerWaitingTimeFunction: () => TimeSpan.FromSeconds(30)
            );
    }
}
