using Grpc.Core;
using System.Reflection;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Ports.ClientWebBFF.Protos.V1;
using MCIO.Observability.Abstractions;
using MCIO.Demos.Store.Ports.ClientWebBFF.Adapters;

namespace MCIO.Demos.Store.Ports.ClientWebBFF.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    // Constants
    public const string PING_TRACE_NAME = "GrpcPing";

    // Fields
    private readonly ITraceManager _traceManager;
    private readonly Gateways.General.Protos.V1.PingService.PingServiceClient _gatewayPingServiceClient;
    private readonly static string _assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

    // Constructors
    public PingGrpcService(
        ITraceManager traceManager,
        Gateways.General.Protos.V1.PingService.PingServiceClient gatewayPingServiceClient
    )
    {
        _traceManager = traceManager;
        _gatewayPingServiceClient = gatewayPingServiceClient;
    }

    // Public Methods
    public override async Task<PingReply> Ping(PingRequest request, ServerCallContext context)
    {
        var executionInfo = ExecutionInfoAdapter.Adapt(request.ExecutionInfo)!.Value;

        return await _traceManager.StartInternalActivityAsync(
            name: PING_TRACE_NAME,
            executionInfo,
            input: request,
            handler: async (activity, executionInfo, input, cancellationToken) =>
            {
                var reply = new PingReply();

                var gatewayPingReply = await _gatewayPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken);

                foreach (var replyMessage in gatewayPingReply.ReplyMessageCollection)
                    reply.ReplyMessageCollection.Add(replyMessage);

                reply.ReplyMessageCollection.Add(
                    new ReplyMessage
                    {
                        Type = ReplyMessageType.Information,
                        Code = _assemblyName
                    }
                );

                return reply;
            },
            context.CancellationToken
        );
    }
}
