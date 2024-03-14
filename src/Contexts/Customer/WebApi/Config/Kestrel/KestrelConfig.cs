using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Customer.WebApi.Config.Kestrel;

public class KestrelConfig
{
    [Required]
    public int HttpPort { get; set; }

    [Required]
    public int GrpcPort { get; set; }
}
