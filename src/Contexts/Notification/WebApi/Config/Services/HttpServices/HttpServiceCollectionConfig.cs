using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Notification.WebApi.Config.Services.HttpServices;

public class HttpServiceCollectionConfig
{
    [Required]
    public HttpService GeneralGateway { get; set; } = null!;
}
