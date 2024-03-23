using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Services.Interfaces;

public interface IGeneralGatewayService
{
    Task<OutputEnvelop<HttpResponseMessage?>> PingHttpAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
    Task<OutputEnvelop<PingReply?>> PingGrpcAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
}
