using Grpc.Core;
using Grpc.Net.ClientFactory;
using System.Reflection;

namespace MCIO.Demos.Store.Gateways.General.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    // Fields
    private static readonly string _origin = Assembly.GetExecutingAssembly().GetName().Name!;

    private readonly Analytics.WebApi.PingService.PingServiceClient _analyticsPingServiceClient;
    private readonly Basket.WebApi.PingService.PingServiceClient _basketPingServiceClient;
    private readonly Calendar.WebApi.PingService.PingServiceClient _calendarPingServiceClient;
    private readonly Catalog.WebApi.PingService.PingServiceClient _catalogPingServiceClient;
    private readonly Customer.WebApi.PingService.PingServiceClient _customerPingServiceClient;
    private readonly Delivery.WebApi.PingService.PingServiceClient _deliveryPingServiceClient;
    private readonly Identity.WebApi.PingService.PingServiceClient _identityPingServiceClient;
    private readonly Notification.WebApi.PingService.PingServiceClient _notificationPingServiceClient;
    private readonly Order.WebApi.PingService.PingServiceClient _orderPingServiceClient;
    private readonly Payment.WebApi.PingService.PingServiceClient _paymentPingServiceClient;
    private readonly Pricing.WebApi.PingService.PingServiceClient _pricingPingServiceClient;
    private readonly Product.WebApi.PingService.PingServiceClient _productPingServiceClient;

    // Constructors
    public PingGrpcService(
        GrpcClientFactory grpcClientFactory
    )
    {
        _analyticsPingServiceClient = grpcClientFactory.CreateClient<Analytics.WebApi.PingService.PingServiceClient>(name: typeof(Analytics.WebApi.PingService.PingServiceClient).FullName!);
        _basketPingServiceClient = grpcClientFactory.CreateClient<Basket.WebApi.PingService.PingServiceClient>(name: typeof(Basket.WebApi.PingService.PingServiceClient).FullName!);
        _calendarPingServiceClient = grpcClientFactory.CreateClient<Calendar.WebApi.PingService.PingServiceClient>(name: typeof(Calendar.WebApi.PingService.PingServiceClient).FullName!);
        _catalogPingServiceClient = grpcClientFactory.CreateClient<Catalog.WebApi.PingService.PingServiceClient>(name: typeof(Catalog.WebApi.PingService.PingServiceClient).FullName!);
        _customerPingServiceClient = grpcClientFactory.CreateClient<Customer.WebApi.PingService.PingServiceClient>(name: typeof(Customer.WebApi.PingService.PingServiceClient).FullName!);
        _deliveryPingServiceClient = grpcClientFactory.CreateClient<Delivery.WebApi.PingService.PingServiceClient>(name: typeof(Delivery.WebApi.PingService.PingServiceClient).FullName!);
        _identityPingServiceClient = grpcClientFactory.CreateClient<Identity.WebApi.PingService.PingServiceClient>(name: typeof(Identity.WebApi.PingService.PingServiceClient).FullName!);
        _notificationPingServiceClient = grpcClientFactory.CreateClient<Notification.WebApi.PingService.PingServiceClient>(name: typeof(Notification.WebApi.PingService.PingServiceClient).FullName!);
        _orderPingServiceClient = grpcClientFactory.CreateClient<Order.WebApi.PingService.PingServiceClient>(name: typeof(Order.WebApi.PingService.PingServiceClient).FullName!);
        _paymentPingServiceClient = grpcClientFactory.CreateClient<Payment.WebApi.PingService.PingServiceClient>(name: typeof(Payment.WebApi.PingService.PingServiceClient).FullName!);
        _pricingPingServiceClient = grpcClientFactory.CreateClient<Pricing.WebApi.PingService.PingServiceClient>(name: typeof(Pricing.WebApi.PingService.PingServiceClient).FullName!);
        _productPingServiceClient = grpcClientFactory.CreateClient<Product.WebApi.PingService.PingServiceClient>(name: typeof(Product.WebApi.PingService.PingServiceClient).FullName!);
    }

    public override async Task<PingReply> Ping(PingRequest request, ServerCallContext context)
    {
        var taskCollection = new Task[]
        {
            _analyticsPingServiceClient.PingAsync(new Analytics.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _basketPingServiceClient.PingAsync(new Basket.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _calendarPingServiceClient.PingAsync(new Calendar.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _catalogPingServiceClient.PingAsync(new Catalog.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _customerPingServiceClient.PingAsync(new Customer.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _deliveryPingServiceClient.PingAsync(new Delivery.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _identityPingServiceClient.PingAsync(new Identity.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _notificationPingServiceClient.PingAsync(new Notification.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _orderPingServiceClient.PingAsync(new Order.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _paymentPingServiceClient.PingAsync(new Payment.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _pricingPingServiceClient.PingAsync(new Pricing.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync,
            _productPingServiceClient.PingAsync(new Product.WebApi.PingRequest { Origin = _origin }, cancellationToken: context.CancellationToken).ResponseAsync
        };

        await Task.WhenAll(taskCollection);

        return new PingReply
        {
            Origin = request.Origin,
            Server = _origin
        };
    }
}
