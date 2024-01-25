using MCIO.Demos.Store.Customer.WebApi.Config.HealthCheck;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Customer.WebApi.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;
}
