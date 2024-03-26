﻿using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions.Models;
using MCIO.Demos.Store.BuildingBlock.Resilience;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Order.Interfaces;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Order;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Payment.Interfaces;

namespace MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Payment;

public class PaymentPingHttpOperationResiliencePolicy
    : ResiliencePolicyBase,
    IPaymentPingHttpOperationResiliencePolicy
{
    protected override void ConfigureInternal(ResiliencePolicyOptions options)
    {
        options
            .WithCustomIdentificationOptions(
                name: nameof(PaymentPingHttpOperationResiliencePolicy)
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