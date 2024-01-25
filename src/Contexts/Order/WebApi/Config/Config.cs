using MCIO.Demos.Store.Order.WebApi.Config.HealthCheck;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Order.WebApi.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;
}
