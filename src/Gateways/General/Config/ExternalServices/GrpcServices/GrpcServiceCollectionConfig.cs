using MCIO.Demos.Store.BuildingBlock.Grpc.Models;
using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Ports.ClientWebBFF.Config.ExternalServices.GrpcServices;

public class GrpcServiceCollectionConfig
{
    [Required]
    public GrpcServiceConfig AnalyticsContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig BasketContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig CalendarContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig CatalogContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig CustomerContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig DeliveryContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig IdentityContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig NotificationContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig OrderContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig PaymentContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig PricingContext { get; set; } = null!;

    [Required]
    public GrpcServiceConfig ProductContext { get; set; } = null!;
}
