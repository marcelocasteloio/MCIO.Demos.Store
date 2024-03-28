using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Identity.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Identity.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Identity.V1.Models;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.OutputEnvelop.Enums;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Identity.V1;

public class IdentityContextService
    : ContextServiceBase,
    IIdentityContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(IdentityContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(IdentityContextService)}.{nameof(PingGrpcAsync)}";

    private readonly IIdentityPingGrpcOperationResiliencePolicy _identityPingGrpcOperationResiliencePolicy;
    private readonly IIdentityPingHttpOperationResiliencePolicy _identityPingHttpOperationResiliencePolicy;

    private readonly Store.Identity.WebApi.Protos.V1.PingService.PingServiceClient _identityPingServiceClient;

    public IdentityContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        IIdentityPingGrpcOperationResiliencePolicy identityPingGrpcOperationResiliencePolicy,
        IIdentityPingHttpOperationResiliencePolicy identityPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _identityPingServiceClient = grpcClientFactory.CreateClient<Store.Identity.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Identity.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _identityPingGrpcOperationResiliencePolicy = identityPingGrpcOperationResiliencePolicy;
        _identityPingHttpOperationResiliencePolicy = identityPingHttpOperationResiliencePolicy;
    }

    // Public Methods
    public async Task<OutputEnvelop<LoginResponse?>> LoginAsync(
        LoginPayload payload,
        CancellationToken cancellationToken
    )
    {
        var response = await HttpClient.PostAsJsonAsync(
            requestUri: $"{Config.ExternalServices.HttpServiceCollection.IdentityContext.BaseUrl}/api/v1/auth/login",
            value: payload,
            cancellationToken
        );

        return OutputEnvelop<LoginResponse?>.Create(
            output: await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken),
            type: response.IsSuccessStatusCode ? OutputEnvelopType.Success : OutputEnvelopType.Error
        );
    }

    public async Task<OutputEnvelop<bool?>> ValidateTokenAsync(
        string authorizationHeaderValue,
        CancellationToken cancellationToken
    )
    {
        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);

        var response = await HttpClient.GetAsync(
            requestUri: $"{Config.ExternalServices.HttpServiceCollection.IdentityContext.BaseUrl}/api/v1/auth/validate-token",
            cancellationToken
        );

        return OutputEnvelop<bool?>.Create(
            output: response.IsSuccessStatusCode,
            type: response.IsSuccessStatusCode ? OutputEnvelopType.Success : OutputEnvelopType.Error
        );
    }

    public async Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken)
    {
        await HttpClient.GetAsync(
            requestUri: $"{Config.ExternalServices.HttpServiceCollection.IdentityContext.BaseUrl}/api/v1/ping",
            cancellationToken
        );

        return OutputEnvelop.OutputEnvelop.CreateSuccess();
    }
    public override Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(
        Core.ExecutionInfo.ExecutionInfo executionInfo,
        CancellationToken cancellationToken
    )
    {
        return ExecuteHttpRequestWithResponseBaseReturnAsync(
            traceName: _pingHttpAsyncTraceName,
            executionInfo,
            resiliencePolicy: _identityPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.IdentityContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _identityPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _identityPingServiceClient.PingAsync(
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