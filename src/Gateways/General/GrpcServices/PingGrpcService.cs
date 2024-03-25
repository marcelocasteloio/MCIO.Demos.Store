using Grpc.Core;
using Grpc.Net.ClientFactory;
using System.Reflection;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Protos.V1;
using MCIO.Observability.Abstractions;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    // Constants
    public readonly static string PING_TRACE_NAME = $"{Assembly.GetExecutingAssembly().GetName().Name}.GrpcPing";

    // Fields
    private readonly ITraceManager _traceManager;
    private readonly static string _assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

    private readonly Analytics.WebApi.Protos.V1.PingService.PingServiceClient _analyticsPingServiceClient;
    private readonly Basket.WebApi.Protos.V1.PingService.PingServiceClient _basketPingServiceClient;
    private readonly Calendar.WebApi.Protos.V1.PingService.PingServiceClient _calendarPingServiceClient;
    private readonly Catalog.WebApi.Protos.V1.PingService.PingServiceClient _catalogPingServiceClient;
    private readonly Customer.WebApi.Protos.V1.PingService.PingServiceClient _customerPingServiceClient;
    private readonly Delivery.WebApi.Protos.V1.PingService.PingServiceClient _deliveryPingServiceClient;
    private readonly Identity.WebApi.Protos.V1.PingService.PingServiceClient _identityPingServiceClient;
    private readonly Notification.WebApi.Protos.V1.PingService.PingServiceClient _notificationPingServiceClient;
    private readonly Order.WebApi.Protos.V1.PingService.PingServiceClient _orderPingServiceClient;
    private readonly Payment.WebApi.Protos.V1.PingService.PingServiceClient _paymentPingServiceClient;
    private readonly Pricing.WebApi.Protos.V1.PingService.PingServiceClient _pricingPingServiceClient;
    private readonly Product.WebApi.Protos.V1.PingService.PingServiceClient _productPingServiceClient;

    // Constructors
    public PingGrpcService(
        ITraceManager traceManager,
        GrpcClientFactory grpcClientFactory
    )
    {
        _traceManager = traceManager;
        _analyticsPingServiceClient = grpcClientFactory.CreateClient<Analytics.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Analytics.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _basketPingServiceClient = grpcClientFactory.CreateClient<Basket.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Basket.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _calendarPingServiceClient = grpcClientFactory.CreateClient<Calendar.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Calendar.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _catalogPingServiceClient = grpcClientFactory.CreateClient<Catalog.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Catalog.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _customerPingServiceClient = grpcClientFactory.CreateClient<Customer.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Customer.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _deliveryPingServiceClient = grpcClientFactory.CreateClient<Delivery.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Delivery.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _identityPingServiceClient = grpcClientFactory.CreateClient<Identity.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Identity.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _notificationPingServiceClient = grpcClientFactory.CreateClient<Notification.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Notification.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _orderPingServiceClient = grpcClientFactory.CreateClient<Order.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Order.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _paymentPingServiceClient = grpcClientFactory.CreateClient<Payment.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Payment.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _pricingPingServiceClient = grpcClientFactory.CreateClient<Pricing.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Pricing.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
        _productPingServiceClient = grpcClientFactory.CreateClient<Product.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Product.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);
    }

    public override async Task<PingReply> Ping(PingRequest request, ServerCallContext context)
    {
        var executionInfo = ExecutionInfoFactory.Create(request.RequestHeader.ExecutionInfo)!.Value;

        return await _traceManager.StartInternalActivityAsync(
            name: PING_TRACE_NAME,
            executionInfo,
            input: request,
            handler: async (activity, executionInfo, input, cancellationToken) =>
            {
                var reply = new PingReply()
                {
                    ReplyHeader = new ReplyHeader()
                };

                var replyCollection = new List<PingReply>();

                var taskCollection = new Task<PingReply>[] { 
                    PingAnalyticsAsync(activity, request, cancellationToken),
                    PingBasketAsync(activity, request, cancellationToken),
                    PingCalendarAsync(activity, request, cancellationToken),
                    PingCatalogAsync(activity, request, cancellationToken),
                    PingCustomerAsync(activity, request, cancellationToken),
                    PingDeliveryAsync(activity, request, cancellationToken),
                    PingIdentityAsync(activity, request, cancellationToken),
                    PingNotificationAsync(activity, request, cancellationToken),
                    PingOrderAsync(activity, request, cancellationToken),
                    PingPaymentAsync(activity, request, cancellationToken),
                    PingPricingAsync(activity, request, cancellationToken),
                    PingProductAsync(activity, request, cancellationToken),
                };

                await Task.WhenAll(taskCollection);

                foreach (var task in taskCollection)
                {
                    var pingReply = await task;

                    foreach (var replyMessage in pingReply.ReplyHeader.ReplyMessageCollection)
                        reply.ReplyHeader.ReplyMessageCollection.Add(replyMessage);
                }

                reply.ReplyHeader.ReplyMessageCollection.Add(
                    new ReplyMessage
                    {
                        Type = ReplyMessageType.Information,
                        Code = _assemblyName
                    }
                );

                return reply;
            },
            context.CancellationToken
        );
    }

    private async Task<PingReply> PingAnalyticsAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Analytics start"));

        var reply = await _analyticsPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Analytics end"));

        return reply;
    }
    private async Task<PingReply> PingBasketAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Basket start"));

        var reply = await _basketPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Basket end"));

        return reply;
    }
    private async Task<PingReply> PingCalendarAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Calendar start"));

        var reply = await _calendarPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Calendar end"));

        return reply;
    }
    private async Task<PingReply> PingCatalogAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Catalog start"));

        var reply = await _catalogPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Catalog end"));

        return reply;
    }
    private async Task<PingReply> PingCustomerAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Customer start"));

        var reply = await _customerPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Customer end"));

        return reply;
    }
    private async Task<PingReply> PingDeliveryAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Delivery start"));

        var reply = await _deliveryPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Delivery end"));

        return reply;
    }
    private async Task<PingReply> PingIdentityAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Identity start"));

        var reply = await _identityPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Identity end"));

        return reply;
    }
    private async Task<PingReply> PingNotificationAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Notification start"));

        var reply = await _notificationPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Notification end"));

        return reply;
    }
    private async Task<PingReply> PingOrderAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Order start"));

        var reply = await _orderPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Order end"));

        return reply;
    }
    private async Task<PingReply> PingPaymentAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Payment start"));

        var reply = await _paymentPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Payment end"));

        return reply;
    }
    private async Task<PingReply> PingPricingAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Pricing start"));

        var reply = await _pricingPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Pricing end"));

        return reply;
    }
    private async Task<PingReply> PingProductAsync(System.Diagnostics.Activity activity, PingRequest request, CancellationToken cancellationToken)
    {
        activity.AddEvent(new System.Diagnostics.ActivityEvent("Product start"));

        var reply = await _productPingServiceClient.PingAsync(request, cancellationToken: cancellationToken);

        activity.AddEvent(new System.Diagnostics.ActivityEvent("Product end"));

        return reply;
    }
}
