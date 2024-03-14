using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Pricing.WebApi.Config.Services.HttpServices;

public class HttpServiceCollectionConfig
{
    [Required]
    public HttpService GeneralGateway { get; set; } = null!;
}
