using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.AdminWebBFF.Config.ExternalServices.HttpServices;

public class HttpServiceCollectionConfig
{
    [Required]
    public HttpService GeneralGateway { get; set; } = null!;
}
