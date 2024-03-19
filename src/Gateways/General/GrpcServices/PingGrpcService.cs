using Grpc.Core;
using Grpc.Net.ClientFactory;
using System.Reflection;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Protos.V1;
using MCIO.Observability.Abstractions;
using MCIO.Core.ExecutionInfo;
using MCIO.Demos.Store.Gateways.General.Adapters;

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
        var executionInfo = ExecutionInfoAdapter.Adapt(request.ExecutionInfo)!.Value;

        return await _traceManager.StartInternalActivityAsync(
            name: PING_TRACE_NAME,
            executionInfo,
            input: request,
            handler: async (activity, executionInfo, input, cancellationToken) =>
            {
                var reply = new PingReply();

                var replyCollection = new List<PingReply>();

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Analytics start"));
                replyCollection.Add(await _analyticsPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Analytics end"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Basket start"));
                replyCollection.Add(await _basketPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Basket end"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Calendar start"));
                replyCollection.Add(await _calendarPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Calendar end"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Catalog start"));
                replyCollection.Add(await _catalogPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Catalog end"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Customer start"));
                replyCollection.Add(await _customerPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Customer end"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Delivery start"));
                replyCollection.Add(await _deliveryPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Delivery end"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Identity start"));
                replyCollection.Add(await _identityPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Identity end"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Notification start"));
                replyCollection.Add(await _notificationPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Notification end"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Order start"));
                replyCollection.Add(await _orderPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Order end"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Payment start"));
                replyCollection.Add(await _paymentPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Payment end"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Pricing start"));
                replyCollection.Add(await _pricingPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Pricing start"));

                activity.AddEvent(new System.Diagnostics.ActivityEvent("Product start"));
                replyCollection.Add(await _productPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken));
                activity.AddEvent(new System.Diagnostics.ActivityEvent("Product end"));

                foreach (var item in replyCollection)
                {
                    foreach (var replyMessage in item.ReplyMessageCollection)
                        reply.ReplyMessageCollection.Add(replyMessage);
                }

                reply.ReplyMessageCollection.Add(
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
}
