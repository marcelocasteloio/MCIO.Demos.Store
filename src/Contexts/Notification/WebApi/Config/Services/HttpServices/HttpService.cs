using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Notification.WebApi.Config.Services.HttpServices;

public class HttpService
{
    [Required]
    public string BaseUrl { get; set; } = null!;
}
