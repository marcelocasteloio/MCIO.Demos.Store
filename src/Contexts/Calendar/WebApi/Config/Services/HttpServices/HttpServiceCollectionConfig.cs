using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Calendar.WebApi.Config.Services.HttpServices;

public class HttpServiceCollectionConfig
{
    [Required]
    public HttpService GeneralGateway { get; set; } = null!;
}
