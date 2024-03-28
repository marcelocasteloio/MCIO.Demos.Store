using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Basket.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Basket.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Basket.V1;

public class BasketContextService
    : ContextServiceBase,
    IBasketContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(BasketContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(BasketContextService)}.{nameof(PingGrpcAsync)}";

    private readonly IBasketPingGrpcOperationResiliencePolicy _basketPingGrpcOperationResiliencePolicy;
    private readonly IBasketPingHttpOperationResiliencePolicy _basketPingHttpOperationResiliencePolicy;

    private readonly Store.Basket.WebApi.Protos.V1.PingService.PingServiceClient _basketPingServiceClient;

    // Constructors
    public BasketContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        IBasketPingGrpcOperationResiliencePolicy basketPingGrpcOperationResiliencePolicy,
        IBasketPingHttpOperationResiliencePolicy basketPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _basketPingServiceClient = grpcClientFactory.CreateClient<Store.Basket.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Basket.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _basketPingGrpcOperationResiliencePolicy = basketPingGrpcOperationResiliencePolicy;
        _basketPingHttpOperationResiliencePolicy = basketPingHttpOperationResiliencePolicy;
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
            resiliencePolicy: _basketPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.BasketContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _basketPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _basketPingServiceClient.PingAsync(
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

