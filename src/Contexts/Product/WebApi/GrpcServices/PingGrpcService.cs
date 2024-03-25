using Grpc.Core;
using System.Reflection;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Product.WebApi.Protos.V1;
using MCIO.Observability.Abstractions;
using MCIO.Demos.Store.Product.WebApi.Factories;

namespace MCIO.Demos.Store.Product.WebApi.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    // Constants
    public readonly static string PING_TRACE_NAME = $"{Assembly.GetExecutingAssembly().GetName().Name}.GrpcPing";

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
        var executionInfo = ExecutionInfoFactory.Create(request.RequestHeader.ExecutionInfo)!.Value;

        return await _traceManager.StartInternalActivityAsync(
            name: PING_TRACE_NAME,
            executionInfo,
            input: request,
            handler: (activity, executionInfo, input, cancellationToken) =>
            {
                var reply = new PingReply()
                {
                    ReplyHeader = new ReplyHeader()
                };

                reply.ReplyHeader.ReplyMessageCollection.Add(
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
