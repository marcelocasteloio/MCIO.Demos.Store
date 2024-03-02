using MCIO.Demos.Store.Ports.ClientMobileBFF.Config.Services.HttpServices;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.ClientMobileBFF.Config.Services;

public class ServicesConfig
{
    [Required]
    public HttpServiceCollectionConfig HttpServiceCollection { get; set; } = null!;
}
