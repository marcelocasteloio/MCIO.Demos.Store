using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Order.WebApi.Config.Services.HttpServices;

public class HttpServiceCollectionConfig
{
    [Required]
    public HttpService GeneralGateway { get; set; } = null!;
}
