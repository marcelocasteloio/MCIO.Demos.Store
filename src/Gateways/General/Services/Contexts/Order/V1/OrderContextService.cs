using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Order.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Order.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Order.V1;

public class OrderContextService
    : ContextServiceBase,
    IOrderContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(OrderContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(OrderContextService)}.{nameof(PingGrpcAsync)}";

    private readonly IOrderPingGrpcOperationResiliencePolicy _orderPingGrpcOperationResiliencePolicy;
    private readonly IOrderPingHttpOperationResiliencePolicy _orderPingHttpOperationResiliencePolicy;

    private readonly Store.Order.WebApi.Protos.V1.PingService.PingServiceClient _orderPingServiceClient;

    // Constructors
    public OrderContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        IOrderPingGrpcOperationResiliencePolicy orderPingGrpcOperationResiliencePolicy,
        IOrderPingHttpOperationResiliencePolicy orderPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _orderPingServiceClient = grpcClientFactory.CreateClient<Store.Order.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Order.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _orderPingGrpcOperationResiliencePolicy = orderPingGrpcOperationResiliencePolicy;
        _orderPingHttpOperationResiliencePolicy = orderPingHttpOperationResiliencePolicy;
    }

    // Public Methods
    public override Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(
        Core.ExecutionInfo.ExecutionInfo executionInfo,
        CancellationToken cancellationToken
    )
    {
        return ExecuteHttpRequestWithResponseBaseReturnAsync(
            traceName: _pingHttpAsyncTraceName,
            executionInfo,
            resiliencePolicy: _orderPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.OrderContext.BaseUrl}/api/v1/ping",
                    cancellationToken
                );
            },
            cancellationToken
        );
    }
    public override Task<OutputEnvelop<PingReply?>> PingGrpcAsync(
        Core.ExecutionInfo.ExecutionInfo executionInfo,
        CancellationToken cancellationToken
    )
    {
        return ExecuteGrpcRequestWithReplyHeaderReturnAsync(
            traceName: _pingGrpcAsyncTraceName,
            executionInfo,
            resiliencePolicy: _orderPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _orderPingServiceClient.PingAsync(
                    new PingRequest
                    {
                        RequestHeader = new RequestHeader
                        {
                            ExecutionInfo = ExecutionInfoFactory.Create(executionInfo),
                        }
                    },
                    cancellationToken: cancellationToken
                );

                return (pingReply, pingReply.ReplyHeader);
            },
            cancellationToken
        );
    }
}

