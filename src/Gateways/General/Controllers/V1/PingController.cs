using Asp.Versioning;
using MCIO.Demos.Store.BuildingBlock.WebApi.Controllers;
using MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Analytics.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Basket.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Calendar.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Catalog.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Customer.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Delivery.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Identity.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Notification.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Order.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Payment.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Pricing.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Product.V1.Interfaces;
using MCIO.Observability.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Gateways.General.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[AllowAnonymous]
public class PingController
    : CustomControllerBase
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
        ILogger<PingController> logger,
        ITraceManager traceManager,
        IExecutionInfoAccessor executionInfoAccessor,
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
    ) : base(logger, traceManager, executionInfoAccessor)
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
        return await ExecuteRequestAsync(
            handler: async (executionInfo, activity, cancellationToken) =>
            {
                var taskCollection = new Task[] {
                    _analyticsContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _basketContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _calendarContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _catalogContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _customerContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _deliveryContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _identityContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _notificationContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _orderContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _paymentContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _pricingContextService.PingHttpAsync(executionInfo, cancellationToken),
                    _productContextService.PingHttpAsync(executionInfo, cancellationToken)
                };

                await Task.WhenAll(taskCollection);

                return OutputEnvelop.OutputEnvelop.Create(
                    await _analyticsContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _basketContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _calendarContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _catalogContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _customerContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _deliveryContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _identityContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _notificationContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _orderContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _paymentContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _pricingContextService.PingHttpAsync(executionInfo, cancellationToken),
                    await _productContextService.PingHttpAsync(executionInfo, cancellationToken)
                );
            },
            successStatusCode: 200,
            failStatusCode: 422,
            cancellationToken
        );
    }
}
