using MCIO.Demos.Store.Gateways.General.Config.ExternalServices.HttpServices;
using MCIO.Demos.Store.Ports.ClientWebBFF.Config.ExternalServices.GrpcServices;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Gateways.General.Config.ExternalServices;

public class ExternalServicesConfig
{
    [Required]
    public HttpServiceCollectionConfig HttpServiceCollection { get; set; } = null!;

    [Required]
    public GrpcServiceCollectionConfig GrpcServiceCollection { get; set; } = null!;
}
