using MCIO.Core.ExecutionInfo;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Adapters;
using MCIO.Demos.Store.Ports.AdminMobileBFF.ResiliencePolicies.Interfaces;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Services.Interfaces;
using Polly;
using System.Reflection;

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
    public async Task PingGrpcAsync(ExecutionInfo executionInfo, CancellationToken cancellationToken)
    {
        await _generalGatewayPingGrpcOperationResiliencePolicy.ExecuteAsync(
            handler: async cancellationToken =>
            {
                await _gatewayPingServiceClient.PingAsync(
                    request: new Commom.Protos.V1.PingRequest
                    {
                        ExecutionInfo = ExecutionInfoAdapter.Adapt(executionInfo)
                    },
                    cancellationToken: cancellationToken
                );

                return OutputEnvelop.OutputEnvelop.CreateSuccess();
            },
            cancellationToken: cancellationToken
        );
    }
}
