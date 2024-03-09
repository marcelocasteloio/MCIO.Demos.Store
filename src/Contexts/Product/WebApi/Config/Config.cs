using MCIO.Demos.Store.Product.WebApi.Config.HealthCheck;
using MCIO.Demos.Store.Product.WebApi.Config.Kestrel;
using MCIO.Demos.Store.Product.WebApi.Config.OpenTelemetry;
using MCIO.Demos.Store.Product.WebApi.Config.Services;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Product.WebApi.Config;

public class Config
{
    [Required]
    public KestrelConfig Kestrel { get; set; } = null!;

    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;

    [Required]
    public OpenTelemetryConfig OpenTelemetry { get; set; } = null!;

    [Required]
    public ServicesConfig Services { get; set; } = null!;
}
