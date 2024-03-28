using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Catalog.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Catalog.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Catalog.V1;

public class CatalogContextService
    : ContextServiceBase,
    ICatalogContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(CatalogContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(CatalogContextService)}.{nameof(PingGrpcAsync)}";

    private readonly ICatalogPingGrpcOperationResiliencePolicy _catalogPingGrpcOperationResiliencePolicy;
    private readonly ICatalogPingHttpOperationResiliencePolicy _catalogPingHttpOperationResiliencePolicy;

    private readonly Store.Catalog.WebApi.Protos.V1.PingService.PingServiceClient _catalogPingServiceClient;

    // Constructors
    public CatalogContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        ICatalogPingGrpcOperationResiliencePolicy catalogPingGrpcOperationResiliencePolicy,
        ICatalogPingHttpOperationResiliencePolicy catalogPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _catalogPingServiceClient = grpcClientFactory.CreateClient<Store.Catalog.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Catalog.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _catalogPingGrpcOperationResiliencePolicy = catalogPingGrpcOperationResiliencePolicy;
        _catalogPingHttpOperationResiliencePolicy = catalogPingHttpOperationResiliencePolicy;
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
            resiliencePolicy: _catalogPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.CatalogContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _catalogPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _catalogPingServiceClient.PingAsync(
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

