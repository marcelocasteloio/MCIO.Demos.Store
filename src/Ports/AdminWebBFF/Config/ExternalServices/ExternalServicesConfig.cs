using System.ComponentModel.DataAnnotations;
using MCIO.Demos.Store.Ports.AdminWebBFF.Config.ExternalServices.GrpcServices;
using MCIO.Demos.Store.Ports.AdminWebBFF.Config.ExternalServices.HttpServices;

namespace MCIO.Demos.Store.Ports.AdminWebBFF.Config.ExternalServices;

public class ExternalServicesConfig
{
    [Required]
    public HttpServiceCollectionConfig HttpServiceCollection { get; set; } = null!;

    [Required]
    public GrpcServiceCollectionConfig GrpcServiceCollection { get; set; } = null!;
}
