using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Analytics.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Analytics.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Gateways.General.Factories;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Analytics.V1;

public class AnalyticsContextService
    : ContextServiceBase,
    IAnalyticsContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(AnalyticsContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(AnalyticsContextService)}.{nameof(PingGrpcAsync)}";

    private readonly IAnalyticsPingGrpcOperationResiliencePolicy _analyticsPingGrpcOperationResiliencePolicy;
    private readonly IAnalyticsPingHttpOperationResiliencePolicy _analyticsPingHttpOperationResiliencePolicy;

    private readonly Store.Analytics.WebApi.Protos.V1.PingService.PingServiceClient _analyticsPingServiceClient;

    // Constructors
    public AnalyticsContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        IAnalyticsPingGrpcOperationResiliencePolicy analyticsPingGrpcOperationResiliencePolicy,
        IAnalyticsPingHttpOperationResiliencePolicy analyticsPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _analyticsPingServiceClient = grpcClientFactory.CreateClient<Store.Analytics.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Analytics.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _analyticsPingGrpcOperationResiliencePolicy = analyticsPingGrpcOperationResiliencePolicy;
        _analyticsPingHttpOperationResiliencePolicy = analyticsPingHttpOperationResiliencePolicy;
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
            resiliencePolicy: _analyticsPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.AnalyticsContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _analyticsPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _analyticsPingServiceClient.PingAsync(
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
