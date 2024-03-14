using MCIO.Demos.Store.Ports.AdminWebBFF.Config.ExternalServices;
using MCIO.Demos.Store.Ports.AdminWebBFF.Config.HealthCheck;
using MCIO.Demos.Store.Ports.AdminWebBFF.Config.Kestrel;
using MCIO.Demos.Store.Ports.AdminWebBFF.Config.OpenTelemetry;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.AdminWebBFF.Config;

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
