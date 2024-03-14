using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Delivery.WebApi.Config.Kestrel;

public class KestrelConfig
{
    [Required]
    public int HttpPort { get; set; }

    [Required]
    public int GrpcPort { get; set; }
}
