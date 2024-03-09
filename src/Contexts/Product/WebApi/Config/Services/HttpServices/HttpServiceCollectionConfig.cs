using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Product.WebApi.Config.Services.HttpServices;

public class HttpServiceCollectionConfig
{
    [Required]
    public HttpService GeneralGateway { get; set; } = null!;
}
