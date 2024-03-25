using Asp.Versioning;
using MCIO.Demos.Store.Gateways.General.Services.Analytics.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Basket.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Calendar.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Catalog.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Customer.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Delivery.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Identity.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Notification.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Order.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Payment.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Pricing.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Product.V1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Gateways.General.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class PingController
    : ControllerBase
{
    // Fields
    private readonly IAnalyticsContextService _analyticsContextService;
    private readonly IBasketContextService _basketContextService;
    private readonly ICalendarContextService _calendarContextService;
    private readonly ICatalogContextService _catalogContextService;
    private readonly ICustomerContextService _customerContextService;
    private readonly IDeliveryContextService _deliveryContextService;
    private readonly IIdentityContextService _identityContextService;
    private readonly INotificationContextService _notificationContextService;
    private readonly IOrderContextService _orderContextService;
    private readonly IPaymentContextService _paymentContextService;
    private readonly IPricingContextService _pricingContextService;
    private readonly IProductContextService _productContextService;

    // Constructors
    public PingController(
        IAnalyticsContextService analyticsContextService,
        IBasketContextService basketContextService,
        ICalendarContextService calendarContextService,
        ICatalogContextService catalogContextService,
        ICustomerContextService customerContextService,
        IDeliveryContextService deliveryContextService,
        IIdentityContextService identityContextService,
        INotificationContextService notificationContextService,
        IOrderContextService orderContextService,
        IPaymentContextService paymentContextService,
        IPricingContextService pricingContextService,
        IProductContextService productContextService
    )
    {
        _analyticsContextService = analyticsContextService;
        _basketContextService = basketContextService;
        _calendarContextService = calendarContextService;
        _catalogContextService = catalogContextService;
        _customerContextService = customerContextService;
        _deliveryContextService = deliveryContextService;
        _identityContextService = identityContextService;
        _notificationContextService = notificationContextService;
        _orderContextService = orderContextService;
        _paymentContextService = paymentContextService;
        _pricingContextService = pricingContextService;
        _productContextService = productContextService;
    }

    [HttpGet]
    public async Task<IActionResult> PingAsync(CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            _analyticsContextService.PingHttpAsync(cancellationToken),
            _basketContextService.PingHttpAsync(cancellationToken),
            _calendarContextService.PingHttpAsync(cancellationToken),
            _catalogContextService.PingHttpAsync(cancellationToken),
            _customerContextService.PingHttpAsync(cancellationToken),
            _deliveryContextService.PingHttpAsync(cancellationToken),
            _identityContextService.PingHttpAsync(cancellationToken),
            _notificationContextService.PingHttpAsync(cancellationToken),
            _orderContextService.PingHttpAsync(cancellationToken),
            _paymentContextService.PingHttpAsync(cancellationToken),
            _pricingContextService.PingHttpAsync(cancellationToken),
            _productContextService.PingHttpAsync(cancellationToken)
        );

        return Ok();
    }
}
