using Grpc.Core;
using System.Reflection;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Protos.V1;
using MCIO.Observability.Abstractions;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Adapters;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Services.Interfaces;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    // Constants
    public readonly static string PING_TRACE_NAME = $"{Assembly.GetExecutingAssembly().GetName().Name}.GrpcPing";

    // Fields
    private readonly ITraceManager _traceManager;
    private readonly IGeneralGatewayService _generalGatewayService;
    private readonly static string _assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;

    // Constructors
    public PingGrpcService(
        ITraceManager traceManager,
        IGeneralGatewayService generalGatewayService
    )
    {
        _traceManager = traceManager;
        _generalGatewayService = generalGatewayService;
    }

    // Public Methods
    public override async Task<PingReply> Ping(PingRequest request, ServerCallContext context)
    {
        var executionInfo = ExecutionInfoAdapter.Adapt(request.ExecutionInfo)!.Value;

        var pingOutput = await _traceManager.StartInternalActivityAsync<PingRequest, OutputEnvelop<PingReply?>>(
            name: PING_TRACE_NAME,
            executionInfo,
            input: request,
            handler: async (activity, executionInfo, input, cancellationToken) =>
            {
                var reply = new PingReply();

                var pingGrpcOutput = await _generalGatewayService.PingGrpcAsync(executionInfo, cancellationToken);

                if (!pingGrpcOutput.IsSuccess)
                    return pingGrpcOutput;

                foreach (var replyMessage in pingGrpcOutput.Output!.ReplyMessageCollection)
                    reply.ReplyMessageCollection.Add(replyMessage);

                reply.ReplyMessageCollection.Add(
                    new ReplyMessage
                    {
                        Type = ReplyMessageType.Information,
                        Code = _assemblyName
                    }
                );

                return OutputEnvelop<PingReply?>.CreateSuccess(reply);
            },
            context.CancellationToken
        );

        if(!pingOutput.IsSuccess)
        {
            var reply = new PingReply();

            foreach (var outputMessage in pingOutput.OutputMessageCollection)
            {
                reply.ReplyMessageCollection.Add(new ReplyMessage() { 
                    Code = outputMessage.Code,
                    Description = outputMessage.Description,
                    Type = outputMessage.Type switch
                    {
                        OutputEnvelop.Enums.OutputMessageType.Information => ReplyMessageType.Information,
                        OutputEnvelop.Enums.OutputMessageType.Success => ReplyMessageType.Success,
                        OutputEnvelop.Enums.OutputMessageType.Warning => ReplyMessageType.Warning,
                        OutputEnvelop.Enums.OutputMessageType.Error => ReplyMessageType.Error,
                        _ => ReplyMessageType.Information
                    }
                });
            }

            return reply;
        }

        return pingOutput.Output!;
    }
}
