
namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Services.Interfaces;

public interface IGeneralGatewayService
{
    Task PingHttpAsync(CancellationToken cancellationToken);
    Task PingGrpcAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
}
