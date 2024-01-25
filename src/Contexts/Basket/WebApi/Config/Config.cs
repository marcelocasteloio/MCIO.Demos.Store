using MCIO.Demos.Store.Basket.WebApi.Config.HealthCheck;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Basket.WebApi.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;
}
