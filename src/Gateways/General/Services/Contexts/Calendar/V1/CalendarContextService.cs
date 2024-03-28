using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.Gateways.General.ResiliencePolicies.Contexts.Calendar.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Calendar.V1.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Calendar.V1;

public class CalendarContextService
    : ContextServiceBase,
    ICalendarContextService
{
    // Fields
    private static readonly string _pingHttpAsyncTraceName = $"{nameof(CalendarContextService)}.{nameof(PingHttpAsync)}";
    private static readonly string _pingGrpcAsyncTraceName = $"{nameof(CalendarContextService)}.{nameof(PingGrpcAsync)}";

    private readonly ICalendarPingGrpcOperationResiliencePolicy _calendarPingGrpcOperationResiliencePolicy;
    private readonly ICalendarPingHttpOperationResiliencePolicy _calendarPingHttpOperationResiliencePolicy;

    private readonly Store.Calendar.WebApi.Protos.V1.PingService.PingServiceClient _calendarPingServiceClient;

    // Constructors
    public CalendarContextService(
        ITraceManager traceManager,
        HttpClient httpClient,
        GrpcClientFactory grpcClientFactory,
        Config.Config config,
        ICalendarPingGrpcOperationResiliencePolicy calendarPingGrpcOperationResiliencePolicy,
        ICalendarPingHttpOperationResiliencePolicy calendarPingHttpOperationResiliencePolicy
    ) : base(traceManager, httpClient, config)
    {
        _calendarPingServiceClient = grpcClientFactory.CreateClient<Store.Calendar.WebApi.Protos.V1.PingService.PingServiceClient>(name: typeof(Store.Calendar.WebApi.Protos.V1.PingService.PingServiceClient).FullName!);

        _calendarPingGrpcOperationResiliencePolicy = calendarPingGrpcOperationResiliencePolicy;
        _calendarPingHttpOperationResiliencePolicy = calendarPingHttpOperationResiliencePolicy;
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
            resiliencePolicy: _calendarPingHttpOperationResiliencePolicy,
            handler: cancellationToken =>
            {
                return HttpClient.GetAsync(
                    requestUri: $"{Config.ExternalServices.HttpServiceCollection.CalendarContext.BaseUrl}/api/v1/ping",
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
            resiliencePolicy: _calendarPingGrpcOperationResiliencePolicy,
            handler: async cancellationToken =>
            {
                var pingReply = await _calendarPingServiceClient.PingAsync(
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

