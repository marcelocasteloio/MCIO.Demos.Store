﻿using MCIO.Demos.Store.Gateways.General.Config.HealthCheck;
using MCIO.Demos.Store.Gateways.General.Config.OpenTelemetry;
using MCIO.Demos.Store.Gateways.General.Config.Services;
using MCIO.Demos.Store.Gateways.General.Config.Token;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Gateways.General.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;

    [Required]
    public OpenTelemetryConfig OpenTelemetry { get; set; } = null!;

    [Required]
    public ServicesConfig Services { get; set; } = null!;

    [Required]
    public TokenConfig Token { get; set; } = null!;
}
