using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Catalog.WebApi.Config.Services.HttpServices;

public class HttpServiceCollectionConfig
{
    [Required]
    public HttpService GeneralGateway { get; set; } = null!;
}
