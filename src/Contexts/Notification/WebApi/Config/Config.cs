using MCIO.Demos.Store.Notification.WebApi.Config.HealthCheck;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Notification.WebApi.Config;

public class Config
{
    [Required]
    public HealthCheckConfig HealthCheck { get; set; } = null!;
}
