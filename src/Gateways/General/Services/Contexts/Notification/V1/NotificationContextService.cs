using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Notification.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Notification.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Notification.V1;

public class NotificationContextService
    : ContextServiceBase,
    INotificationContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(NotificationContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(NotificationContextService)}.{nameof(PingGrpcAsync)}";

    private readonly INotificationPingGrpcOperationResiliencePolicy _notificationPingGrpcOperationResiliencePolicy;
    private readonly INotificationPingHttpOperationResiliencePolicy _notificationPingHttpOperationResiliencePolicy;

    private readonly Store.Notification.WebApi.Protos.V1.PingService.PingServiceClient _notificationPingServiceClient;

    // Constructors
    public NotificationContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        INotificationPingGrpcOperationResiliencePolicy notificationPingGrpcOperationResiliencePolicy,
        INotificationPingHttpOperationResiliencePolicy notificationPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _notificationPingServiceClient = grpcClientFactory.CreateClient<Store.Notification.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Notification.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _notificationPingGrpcOperationResiliencePolicy = notificationPingGrpcOperationResiliencePolicy;
        _notificationPingHttpOperationResiliencePolicy = notificationPingHttpOperationResiliencePolicy;
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
            resiliencePolicy: _notificationPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.NotificationContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _notificationPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _notificationPingServiceClient.PingAsync(
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

