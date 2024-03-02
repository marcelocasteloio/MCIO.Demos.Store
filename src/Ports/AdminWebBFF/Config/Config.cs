using MCIO.Demos.Store.Ports.AdminWebBFF.Config.HealthCheck;
using MCIO.Demos.Store.Ports.AdminWebBFF.Config.OpenTelemetry;
using MCIO.Demos.Store.Ports.AdminWebBFF.Config.Services;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.AdminWebBFF.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;

    [Required]
    public OpenTelemetryConfig OpenTelemetry { get; set; } = null!;

    [Required]
    public ServicesConfig Services { get; set; } = null!;
}
