using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Notification.WebApi.Config.HealthCheck;

public class HealthCheckConfig
{
    [Required]
    public string StartupPath { get; set; } = string.Empty;
    [Required]
    public string ReadinessPath { get; set; } = string.Empty;
    [Required]
    public string LivenessPath { get; set; } = string.Empty;
}
