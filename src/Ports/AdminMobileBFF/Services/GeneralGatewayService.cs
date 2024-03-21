using MCIO.Core.ExecutionInfo;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Adapters;
using MCIO.Demos.Store.Ports.AdminMobileBFF.ResiliencePolicies.Interfaces;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Services.Interfaces;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Services;

public class GeneralGatewayService
    : IGeneralGatewayService
{
    // Fields
    private readonly HttpClient _httpClient;
    private readonly Gateways.General.Protos.V1.PingService.PingServiceClient _gatewayPingServiceClient;
    private readonly Config.Config _config;
    private readonly IGeneralGatewayPingGrpcOperationResiliencePolicy _generalGatewayPingGrpcOperationResiliencePolicy;

    // Constructors
    public GeneralGatewayService(
        HttpClient httpClient,
        Gateways.General.Protos.V1.PingService.PingServiceClient gatewayPingServiceClient,
        Config.Config config,
        IGeneralGatewayPingGrpcOperationResiliencePolicy generalGatewayPingGrpcOperationResiliencePolicy
    )
    {
        _httpClient = httpClient;
        _config = config;
        _gatewayPingServiceClient = gatewayPingServiceClient;
        _generalGatewayPingGrpcOperationResiliencePolicy = generalGatewayPingGrpcOperationResiliencePolicy;
    }


    // Public Methods
    public async Task PingHttpAsync(CancellationToken cancellationToken)
    {
        await _httpClient.GetAsync(
            requestUri: $"{_config.ExternalServices.HttpServiceCollection.GeneralGateway.BaseUrl}/api/v1/ping",
            cancellationToken
        );
    }
    public Task<OutputEnvelop<Commom.Protos.V1.PingReply?>> PingGrpcAsync(ExecutionInfo executionInfo, CancellationToken cancellationToken)
    {
        return _generalGatewayPingGrpcOperationResiliencePolicy.ExecuteAsync(
            handler: async cancellationToken =>
            {
                var pingReply = await _gatewayPingServiceClient.PingAsync(
                    request: new Commom.Protos.V1.PingRequest
                    {
                        ExecutionInfo = ExecutionInfoAdapter.Adapt(executionInfo)
                    },
                    cancellationToken: cancellationToken
                );

                return OutputEnvelop<Commom.Protos.V1.PingReply?>.CreateSuccess(pingReply);
            },
            cancellationToken: cancellationToken
        );
    }
}
