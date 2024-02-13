using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Gateways.General.Config.Services.HttpServices;

public class HttpService
{
    [Required]
    public string BaseUrl { get; set; } = null!;
}
