using Grpc.Core;
using System.Reflection;

namespace MCIO.Demos.Store.Ports.ClientMobileBFF.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    // Fields
    private readonly Gateways.General.PingService.PingServiceClient _gatewayPingServiceClient;

    // Constructors
    public PingGrpcService(
        Gateways.General.PingService.PingServiceClient gatewayPingServiceClient
    )
    {
        _gatewayPingServiceClient = gatewayPingServiceClient;
    }

    // Public Methods
    public override async Task<PingReply> Ping(PingRequest request, ServerCallContext context)
    {
        await _gatewayPingServiceClient.PingAsync(
            request: new Gateways.General.PingRequest
            {
                Origin = Assembly.GetExecutingAssembly().GetName().Name
            },
            cancellationToken: context.CancellationToken
        );

        return new PingReply
        {
            Origin = request.Origin,
            Server = Assembly.GetExecutingAssembly().GetName().Name
        };
    }
}
