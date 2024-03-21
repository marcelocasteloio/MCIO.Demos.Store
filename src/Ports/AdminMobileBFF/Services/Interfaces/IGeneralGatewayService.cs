
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Services.Interfaces;

public interface IGeneralGatewayService
{
    Task PingHttpAsync(CancellationToken cancellationToken);
    Task<OutputEnvelop<PingReply?>> PingGrpcAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
}
