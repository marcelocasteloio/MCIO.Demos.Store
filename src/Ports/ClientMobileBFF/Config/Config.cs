using MCIO.Demos.Store.Ports.ClientMobileBFF.Config.ExternalServices;
using MCIO.Demos.Store.Ports.ClientMobileBFF.Config.HealthCheck;
using MCIO.Demos.Store.Ports.ClientMobileBFF.Config.Kestrel;
using MCIO.Demos.Store.Ports.ClientMobileBFF.Config.OpenTelemetry;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.ClientMobileBFF.Config;

public class Config
{
    [Required]
    public KestrelConfig Kestrel { get; set; } = null!;

    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;

    [Required]
    public OpenTelemetryConfig OpenTelemetry { get; set; } = null!;

    [Required]
    public ExternalServicesConfig ExternalServices { get; set; } = null!;
}
