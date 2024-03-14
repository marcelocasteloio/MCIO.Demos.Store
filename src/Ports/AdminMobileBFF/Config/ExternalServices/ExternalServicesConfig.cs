using MCIO.Demos.Store.Ports.AdminMobileBFF.Config.ExternalServices.GrpcServices;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Config.ExternalServices;

public class ExternalServicesConfig
{
    [Required]
    public GrpcServiceCollectionConfig GrpcServiceCollection { get; set; } = null!;
}
