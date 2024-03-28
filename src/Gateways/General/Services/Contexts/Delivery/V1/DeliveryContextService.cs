using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Delivery.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Delivery.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Delivery.V1;

public class DeliveryContextService
    : ContextServiceBase,
    IDeliveryContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(DeliveryContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(DeliveryContextService)}.{nameof(PingGrpcAsync)}";

    private readonly IDeliveryPingGrpcOperationResiliencePolicy _deliveryPingGrpcOperationResiliencePolicy;
    private readonly IDeliveryPingHttpOperationResiliencePolicy _deliveryPingHttpOperationResiliencePolicy;

    private readonly Store.Delivery.WebApi.Protos.V1.PingService.PingServiceClient _deliveryPingServiceClient;

    // Constructors
    public DeliveryContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        IDeliveryPingGrpcOperationResiliencePolicy deliveryPingGrpcOperationResiliencePolicy,
        IDeliveryPingHttpOperationResiliencePolicy deliveryPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _deliveryPingServiceClient = grpcClientFactory.CreateClient<Store.Delivery.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Delivery.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _deliveryPingGrpcOperationResiliencePolicy = deliveryPingGrpcOperationResiliencePolicy;
        _deliveryPingHttpOperationResiliencePolicy = deliveryPingHttpOperationResiliencePolicy;
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
            resiliencePolicy: _deliveryPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.DeliveryContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _deliveryPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _deliveryPingServiceClient.PingAsync(
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

