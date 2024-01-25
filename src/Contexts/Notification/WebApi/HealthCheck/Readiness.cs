﻿using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck;

namespace MCIO.Demos.Store.Notification.WebApi.HealthCheck;

public class Readiness
    : HealthCheckBase
{
    // Constants
    public const string NOT_READY = nameof(NOT_READY);

    // Protected Methods
    protected override Task CheckHealthInternalAsync(Dictionary<string, object> serviceStatusDictionary)
    {
        return Task.CompletedTask;
    }
}
