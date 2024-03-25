using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Ports.ClientMobileBFF.Services.Interfaces;

public interface IGeneralGatewayService
{
    Task<OutputEnvelop<HttpResponseMessage?>> PingHttpAsync(MCIO.Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
    Task<OutputEnvelop<PingReply?>> PingGrpcAsync(MCIO.Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
}
