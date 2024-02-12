﻿using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.HealthCheck;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.OpenTelemetry;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.Services;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;

    [Required]
    public OpenTelemetryConfig OpenTelemetry { get; set; } = null!;

    [Required]
    public ServicesConfig Services { get; set; } = null!;
}
