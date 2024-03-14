using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.BuildingBlock.Grpc.Models;
public class GrpcServiceConfig
{
    [Required]
    public string BaseUrl { get; set; } = null!;

    [Required]
    public int PooledConnectionIdleTimeoutInSeconds { get; set; }

    [Required]
    public int KeepAlivePingDelayInSeconds { get; set; }

    [Required]
    public int KeepAlivePingTimeoutInSeconds { get; set; }

    [Required]
    public bool EnableMultipleHttp2Connections { get; set; }
}
