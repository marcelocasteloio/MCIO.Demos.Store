using MCIO.Demos.Store.Ports.ClientWebBFF.Config.ExternalServices;
using MCIO.Demos.Store.Ports.ClientWebBFF.Config.HealthCheck;
using MCIO.Demos.Store.Ports.ClientWebBFF.Config.Kestrel;
using MCIO.Demos.Store.Ports.ClientWebBFF.Config.OpenTelemetry;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.ClientWebBFF.Config;

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
