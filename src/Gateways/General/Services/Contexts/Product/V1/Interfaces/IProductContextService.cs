using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Product.V1.Interfaces;

public interface IProductContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
    Task<OutputEnvelop<Commom.Protos.V1.PingReply?>> PingGrpcAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
}
