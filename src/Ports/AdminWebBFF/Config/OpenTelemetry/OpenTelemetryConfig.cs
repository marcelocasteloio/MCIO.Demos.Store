using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.AdminWebBFF.Config.OpenTelemetry;

public class OpenTelemetryConfig
{
    [Required]
    public string GrpcCollectorReceiverUrl { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public int MaxQueueSize { get; set; }

    [Required]
    [MinLength(1)]
    public int ExporterTimeoutMilliseconds { get; set; }

    [Required]
    [MinLength(1)]
    public int MaxExportBatchSize { get; set; }

    [Required]
    [MinLength(1)]
    public int ScheduledDelayMilliseconds { get; set; }
}
