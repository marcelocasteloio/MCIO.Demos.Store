using MCIO.Demos.Store.Payment.WebApi.Config.Services.HttpServices;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Payment.WebApi.Config.Services;

public class ServicesConfig
{
    [Required]
    public HttpServiceCollectionConfig HttpServiceCollection { get; set; } = null!;
}
