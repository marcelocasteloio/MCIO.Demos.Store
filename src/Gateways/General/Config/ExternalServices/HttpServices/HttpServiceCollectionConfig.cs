using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Gateways.General.Config.ExternalServices.HttpServices;

public class HttpServiceCollectionConfig
{
    [Required]
    public HttpService AnalyticsContext { get; set; } = null!;

    [Required]
    public HttpService BasketContext { get; set; } = null!;

    [Required]
    public HttpService CalendarContext { get; set; } = null!;

    [Required]
    public HttpService CatalogContext { get; set; } = null!;

    [Required]
    public HttpService CustomerContext { get; set; } = null!;

    [Required]
    public HttpService DeliveryContext { get; set; } = null!;

    [Required]
    public HttpService IdentityContext { get; set; } = null!;

    [Required]
    public HttpService NotificationContext { get; set; } = null!;

    [Required]
    public HttpService OrderContext { get; set; } = null!;

    [Required]
    public HttpService PaymentContext { get; set; } = null!;

    [Required]
    public HttpService PricingContext { get; set; } = null!;

    [Required]
    public HttpService ProductContext { get; set; } = null!;
}
