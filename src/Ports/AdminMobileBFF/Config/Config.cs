using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.ExternalServices;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.HealthCheck;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.Kestrel;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.OpenTelemetry;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Config;

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
