using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Product.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Product.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Product.V1;

public class ProductContextService
    : ContextServiceBase,
    IProductContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(ProductContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(ProductContextService)}.{nameof(PingGrpcAsync)}";

    private readonly IProductPingGrpcOperationResiliencePolicy _productPingGrpcOperationResiliencePolicy;
    private readonly IProductPingHttpOperationResiliencePolicy _productPingHttpOperationResiliencePolicy;

    private readonly Store.Product.WebApi.Protos.V1.PingService.PingServiceClient _productPingServiceClient;

    // Constructors
    public ProductContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        IProductPingGrpcOperationResiliencePolicy productPingGrpcOperationResiliencePolicy,
        IProductPingHttpOperationResiliencePolicy productPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _productPingServiceClient = grpcClientFactory.CreateClient<Store.Product.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Product.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _productPingGrpcOperationResiliencePolicy = productPingGrpcOperationResiliencePolicy;
        _productPingHttpOperationResiliencePolicy = productPingHttpOperationResiliencePolicy;
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
            resiliencePolicy: _productPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.ProductContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _productPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _productPingServiceClient.PingAsync(
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

