using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.Services.HttpServices;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Config.Services;

public class ServicesConfig
{
    [Required]
    public HttpServiceCollectionConfig HttpServiceCollection { get; set; } = null!;
}
