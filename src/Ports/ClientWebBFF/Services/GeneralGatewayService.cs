using MCIO.Core.ExecutionInfo;
using MCIO.Demos.Store.Ports.ClientWebBFF.Factories;
using MCIO.Demos.Store.Ports.ClientWebBFF.ResiliencePolicies.Interfaces;
using MCIO.Demos.Store.Ports.ClientWebBFF.Services.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.OutputEnvelop.Enums;

namespace MCIO.Demos.Store.Ports.ClientWebBFF.Services;

public class GeneralGatewayService
    : IGeneralGatewayService
{
    // Constants
    public static readonly string HTTP_PING_TRACE_NAME = $"{nameof(GeneralGatewayService)}.Http";
    public static readonly string GRPC_PING_TRACE_NAME = $"{nameof(GeneralGatewayService)}.Grpc";

    // Fields
    private readonly ITraceManager _traceManager;

    private readonly HttpClient _httpClient;
    private readonly Gateways.General.Protos.V1.PingService.PingServiceClient _gatewayPingServiceClient;

    private readonly IGeneralGatewayPingHttpOperationResiliencePolicy _generalGatewayPingHttpOperationResiliencePolicy;
    private readonly IGeneralGatewayPingGrpcOperationResiliencePolicy _generalGatewayPingGrpcOperationResiliencePolicy;

    private readonly string _httpRequestUri;

    // Constructors
    public GeneralGatewayService(
        ITraceManager traceManager,
        Config.Config config,

        HttpClient httpClient,
        Gateways.General.Protos.V1.PingService.PingServiceClient gatewayPingServiceClient,

        IGeneralGatewayPingHttpOperationResiliencePolicy generalGatewayPingHttpOperationResiliencePolicy,
        IGeneralGatewayPingGrpcOperationResiliencePolicy generalGatewayPingGrpcOperationResiliencePolicy
    )
    {
        _traceManager = traceManager;
        _httpRequestUri = $"{config.ExternalServices.HttpServiceCollection.GeneralGateway.BaseUrl}/api/v1/ping";

        _httpClient = httpClient;
        _gatewayPingServiceClient = gatewayPingServiceClient;

        _generalGatewayPingHttpOperationResiliencePolicy = generalGatewayPingHttpOperationResiliencePolicy;
        _generalGatewayPingGrpcOperationResiliencePolicy = generalGatewayPingGrpcOperationResiliencePolicy;
    }


    // Public Methods
    public Task<OutputEnvelop<HttpResponseMessage?>> PingHttpAsync(ExecutionInfo executionInfo, CancellationToken cancellationToken)
    {
        return _generalGatewayPingHttpOperationResiliencePolicy.ExecuteAsync(
            handler: cancellationToken =>
            {
                return _traceManager.StartInternalActivityAsync(
                    name: HTTP_PING_TRACE_NAME,
                    executionInfo,
                    handler: async (activity, executionInfo, cancellationToken) =>
                    {
                        var response = await _httpClient.GetAsync(
                            requestUri: _httpRequestUri,
                            cancellationToken
                        );

                        return OutputEnvelop<HttpResponseMessage?>.Create(
                            output: response,
                            type: response.IsSuccessStatusCode
                                ? OutputEnvelopType.Success
                                : OutputEnvelopType.Error
                        );
                    },
                    cancellationToken
                );
            },
            cancellationToken
        );
    }
    public Task<OutputEnvelop<Commom.Protos.V1.PingReply?>> PingGrpcAsync(ExecutionInfo executionInfo, CancellationToken cancellationToken)
    {
        return _generalGatewayPingGrpcOperationResiliencePolicy.ExecuteAsync(
            handler: cancellationToken =>
            {
                return _traceManager.StartInternalActivityAsync(
                    name: GRPC_PING_TRACE_NAME,
                    executionInfo,
                    handler: async (activity, executionInfo, cancellationToken) =>
                    {
                        return OutputEnvelop<Commom.Protos.V1.PingReply?>.CreateSuccess(
                            await _gatewayPingServiceClient.PingAsync(
                                request: new Commom.Protos.V1.PingRequest
                                {
                                    RequestHeader = new Commom.Protos.V1.RequestHeader
                                    {
                                        ExecutionInfo = ExecutionInfoFactory.Create(executionInfo)
                                    }
                                },
                                cancellationToken: cancellationToken
                            )
                        );
                    },
                    cancellationToken
                );
            },
            cancellationToken
        );
    }
}
