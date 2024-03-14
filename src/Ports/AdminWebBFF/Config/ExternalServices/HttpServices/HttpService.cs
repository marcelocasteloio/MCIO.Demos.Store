using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.AdminWebBFF.Config.ExternalServices.HttpServices;

public class HttpService
{
    [Required]
    public string BaseUrl { get; set; } = null!;
}
