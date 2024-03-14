using System.ComponentModel.DataAnnotations;
using MCIO.Demos.Store.BuildingBlock.Grpc.Models;

namespace MCIO.Demos.Store.Ports.ClientMobileBFF.Config.ExternalServices.GrpcServices;

public class GrpcServiceCollectionConfig
{
    [Required]
    public GrpcServiceConfig GeneralGateway { get; set; } = null!;
}
