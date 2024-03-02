using MCIO.Demos.Store.Pricing.WebApi.Config.HealthCheck;
using MCIO.Demos.Store.Pricing.WebApi.Config.OpenTelemetry;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Pricing.WebApi.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;

    [Required]
    public OpenTelemetryConfig OpenTelemetry { get; set; } = null!;
}
