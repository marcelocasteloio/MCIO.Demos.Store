using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Payment.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Payment.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Payment.V1;

public class PaymentContextService
    : ContextServiceBase,
    IPaymentContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(PaymentContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(PaymentContextService)}.{nameof(PingGrpcAsync)}";

    private readonly IPaymentPingGrpcOperationResiliencePolicy _paymentPingGrpcOperationResiliencePolicy;
    private readonly IPaymentPingHttpOperationResiliencePolicy _paymentPingHttpOperationResiliencePolicy;

    private readonly Store.Payment.WebApi.Protos.V1.PingService.PingServiceClient _paymentPingServiceClient;

    // Constructors
    public PaymentContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        IPaymentPingGrpcOperationResiliencePolicy paymentPingGrpcOperationResiliencePolicy,
        IPaymentPingHttpOperationResiliencePolicy paymentPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _paymentPingServiceClient = grpcClientFactory.CreateClient<Store.Payment.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Payment.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _paymentPingGrpcOperationResiliencePolicy = paymentPingGrpcOperationResiliencePolicy;
        _paymentPingHttpOperationResiliencePolicy = paymentPingHttpOperationResiliencePolicy;
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
            resiliencePolicy: _paymentPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.PaymentContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _paymentPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _paymentPingServiceClient.PingAsync(
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

