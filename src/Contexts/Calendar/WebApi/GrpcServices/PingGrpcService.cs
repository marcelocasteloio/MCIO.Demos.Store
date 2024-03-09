using Grpc.Core;

namespace MCIO.Demos.Store.Calendar.WebApi.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    public override async Task<PingReply> Ping(PingRequest request, ServerCallContext context)
    {
        await Task.Yield();

        return new PingReply();
    }
}
