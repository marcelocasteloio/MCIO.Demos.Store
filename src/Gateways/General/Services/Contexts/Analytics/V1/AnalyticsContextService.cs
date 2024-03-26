using MCIO.Demos.Store.BuildingBlock.WebApi.Responses;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Analytics.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Analytics.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using System.Diagnostics;
using System.Text.Json;

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


    // Constructors
    public AnalyticsContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        Config.Config config,
        IAnalyticsPingGrpcOperationResiliencePolicy analyticsPingGrpcOperationResiliencePolicy,
        IAnalyticsPingHttpOperationResiliencePolicy analyticsPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _analyticsPingGrpcOperationResiliencePolicy = analyticsPingGrpcOperationResiliencePolicy;
        _analyticsPingHttpOperationResiliencePolicy = analyticsPingHttpOperationResiliencePolicy;
    }

    // Public Methods
    public Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(
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
    public Task<OutputEnvelop<PingReply?>> PingGrpcAsync(
        Core.ExecutionInfo.ExecutionInfo executionInfo,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
