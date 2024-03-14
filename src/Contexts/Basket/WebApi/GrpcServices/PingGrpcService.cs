using Grpc.Core;
using System.Reflection;

namespace MCIO.Demos.Store.Basket.WebApi.GrpcServices;

public class PingGrpcService
    : PingService.PingServiceBase
{
    public override async Task<PingReply> Ping(PingRequest request, ServerCallContext context)
    {
        await Task.Yield();

        return new PingReply
        {
            Origin = request.Origin,
            Server = Assembly.GetExecutingAssembly().GetName().Name
        };
    }
}
