using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.ExternalServices.GrpcServices;
using System.ComponentModel.DataAnnotations;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.ExternalServices.HttpServices;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Config.ExternalServices;

public class ExternalServicesConfig
{
    [Required]
    public HttpServiceCollectionConfig HttpServiceCollection { get; set; } = null!;

    [Required]
    public GrpcServiceCollectionConfig GrpcServiceCollection { get; set; } = null!;
}
