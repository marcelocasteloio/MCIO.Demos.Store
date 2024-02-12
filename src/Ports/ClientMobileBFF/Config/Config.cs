using MCIO.Demos.Store.Ports.ClientMobileBFF.Config.HealthCheck;
using MCIO.Demos.Store.Ports.ClientMobileBFF.Config.OpenTelemetry;
using MCIO.Demos.Store.Ports.ClientMobileBFF.Config.Services;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.ClientMobileBFF.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;

    [Required]
    public OpenTelemetryConfig OpenTelemetry { get; set; } = null!;

    [Required]
    public ServicesConfig Services { get; set; } = null!;
}
