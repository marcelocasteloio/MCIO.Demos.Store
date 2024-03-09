using Grpc.Core;
using MCIO.Demos.Store.Analytics.WebApi;

namespace MCIO.Demos.Store.Analytics.WebApi.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    public override async Task<PingReply> Ping(PingRequest request, ServerCallContext context)
    {
        await Task.Yield();

        return new PingReply();
    }
}
