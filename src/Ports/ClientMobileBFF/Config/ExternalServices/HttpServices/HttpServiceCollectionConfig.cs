using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.ClientMobileBFF.Config.ExternalServices.HttpServices;

public class HttpServiceCollectionConfig
{
    [Required]
    public HttpService GeneralGateway { get; set; } = null!;
}
