using MCIO.Demos.Store.Gateways.General.Config.ExternalServices;
using MCIO.Demos.Store.Gateways.General.Config.HealthCheck;
using MCIO.Demos.Store.Gateways.General.Config.Kestrel;
using MCIO.Demos.Store.Gateways.General.Config.OpenTelemetry;
using MCIO.Demos.Store.Gateways.General.Config.Token;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Gateways.General.Config;

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

    [Required]
    public TokenConfig Token { get; set; } = null!;
}
