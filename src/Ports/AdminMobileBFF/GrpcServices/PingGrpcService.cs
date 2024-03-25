﻿using Grpc.Core;
using System.Reflection;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Observability.Abstractions;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Protos.V1;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Factories;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    // Constants
    public readonly static string PING_TRACE_NAME = $"{Assembly.GetExecutingAssembly().GetName().Name}.GrpcPing";

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
        var executionInfo = ExecutionInfoFactory.Create(request.RequestHeader.ExecutionInfo)!.Value;

        return await _traceManager.StartInternalActivityAsync(
            name: PING_TRACE_NAME,
            executionInfo,
            input: request,
            handler: async (activity, executionInfo, input, cancellationToken) =>
            {
                var reply = new PingReply()
                {
                    ReplyHeader = new ReplyHeader()
                };

                var gatewayPingReply = await _gatewayPingServiceClient.PingAsync(request, cancellationToken: context.CancellationToken);

                foreach (var replyMessage in gatewayPingReply.ReplyHeader.ReplyMessageCollection)
                    reply.ReplyHeader.ReplyMessageCollection.Add(replyMessage);

                reply.ReplyHeader.ReplyMessageCollection.Add(
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
