using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.ClientMobileBFF.Config.ExternalServices.HttpServices;

public class HttpService
{
    [Required]
    public string BaseUrl { get; set; } = null!;
}
