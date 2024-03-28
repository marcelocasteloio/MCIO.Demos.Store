using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Customer.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Customer.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Customer.V1;

public class CustomerContextService
    : ContextServiceBase,
    ICustomerContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(CustomerContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(CustomerContextService)}.{nameof(PingGrpcAsync)}";

    private readonly ICustomerPingGrpcOperationResiliencePolicy _customerPingGrpcOperationResiliencePolicy;
    private readonly ICustomerPingHttpOperationResiliencePolicy _customerPingHttpOperationResiliencePolicy;

    private readonly Store.Customer.WebApi.Protos.V1.PingService.PingServiceClient _customerPingServiceClient;

    // Constructors
    public CustomerContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        ICustomerPingGrpcOperationResiliencePolicy customerPingGrpcOperationResiliencePolicy,
        ICustomerPingHttpOperationResiliencePolicy customerPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _customerPingServiceClient = grpcClientFactory.CreateClient<Store.Customer.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Customer.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _customerPingGrpcOperationResiliencePolicy = customerPingGrpcOperationResiliencePolicy;
        _customerPingHttpOperationResiliencePolicy = customerPingHttpOperationResiliencePolicy;
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
            resiliencePolicy: _customerPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.CustomerContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _customerPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _customerPingServiceClient.PingAsync(
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

