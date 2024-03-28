using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Base.Interfaces;

public interface IContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
    Task<OutputEnvelop<Commom.Protos.V1.PingReply?>> PingGrpcAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
}
