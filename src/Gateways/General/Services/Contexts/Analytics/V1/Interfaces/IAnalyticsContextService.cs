using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Analytics.V1.Interfaces;

public interface IAnalyticsContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
    Task<OutputEnvelop<Commom.Protos.V1.PingReply?>> PingGrpcAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
}
