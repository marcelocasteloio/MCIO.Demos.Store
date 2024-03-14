using System.ComponentModel.DataAnnotations;
using MCIO.Demos.Store.Ports.ClientWebBFF.Config.ExternalServices.HttpServices;
using MCIO.Demos.Store.Ports.ClientWebBFF.Config.ExternalServices.GrpcServices;

namespace MCIO.Demos.Store.Ports.ClientWebBFF.Config.ExternalServices;

public class ExternalServicesConfig
{
    [Required]
    public HttpServiceCollectionConfig HttpServiceCollection { get; set; } = null!;

    [Required]
    public GrpcServiceCollectionConfig GrpcServiceCollection { get; set; } = null!;
}
