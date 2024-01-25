
using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.HealthCheck;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;
}
