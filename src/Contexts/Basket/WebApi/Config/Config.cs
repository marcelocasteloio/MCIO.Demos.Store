using MCIO.Demos.Store.Basket.WebApi.Config.HealthCheck;
using MCIO.Demos.Store.Basket.WebApi.Config.OpenTelemetry;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Basket.WebApi.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;

    [Required]
    public OpenTelemetryConfig OpenTelemetry { get; set; } = null!;
}
