﻿using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions;

namespace MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Payment.Interfaces;

public interface IPaymentPingGrpcOperationResiliencePolicy
    : IResiliencePolicy
{
}
