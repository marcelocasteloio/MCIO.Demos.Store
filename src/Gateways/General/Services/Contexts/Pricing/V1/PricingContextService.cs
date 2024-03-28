using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Pricing.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Pricing.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Pricing.V1;

public class PricingContextService
    : ContextServiceBase,
    IPricingContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(PricingContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(PricingContextService)}.{nameof(PingGrpcAsync)}";

    private readonly IPricingPingGrpcOperationResiliencePolicy _pricingPingGrpcOperationResiliencePolicy;
    private readonly IPricingPingHttpOperationResiliencePolicy _pricingPingHttpOperationResiliencePolicy;

    private readonly Store.Pricing.WebApi.Protos.V1.PingService.PingServiceClient _pricingPingServiceClient;

    // Constructors
    public PricingContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        IPricingPingGrpcOperationResiliencePolicy pricingPingGrpcOperationResiliencePolicy,
        IPricingPingHttpOperationResiliencePolicy pricingPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _pricingPingServiceClient = grpcClientFactory.CreateClient<Store.Pricing.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Pricing.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _pricingPingGrpcOperationResiliencePolicy = pricingPingGrpcOperationResiliencePolicy;
        _pricingPingHttpOperationResiliencePolicy = pricingPingHttpOperationResiliencePolicy;
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
            resiliencePolicy: _pricingPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.PricingContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _pricingPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _pricingPingServiceClient.PingAsync(
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

