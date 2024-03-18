using Grpc.Core;
using System.Reflection;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Calendar.WebApi.Protos.V1;
using MCIO.Observability.Abstractions;
using MCIO.Demos.Store.Calendar.WebApi.Adapters;

namespace MCIO.Demos.Store.Calendar.WebApi.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    // Constants
    public const string PING_TRACE_NAME = "GrpcPing";

    // Fields
    private readonly ITraceManager _traceManager;
    private readonly static string _assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

    // Constructors
    public PingGrpcService(ITraceManager traceManager)
    {
        _traceManager = traceManager;
    }

    // Public Methods
    public override async Task<PingReply> Ping(PingRequest request, ServerCallContext context)
    {
        var executionInfo = ExecutionInfoAdapter.Adapt(request.ExecutionInfo)!.Value;

        return await _traceManager.StartInternalActivityAsync(
            name: PING_TRACE_NAME,
            executionInfo,
            input: request,
            handler: (activity, executionInfo, input, cancellationToken) =>
            {
                var reply = new PingReply();

                reply.ReplyMessageCollection.Add(
                    new ReplyMessage
                    {
                        Type = ReplyMessageType.Information,
                        Code = _assemblyName
                    }
                );

                return Task.FromResult(reply);
            },
            context.CancellationToken
        );
    }
}
