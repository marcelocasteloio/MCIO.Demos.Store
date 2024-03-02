
namespace MCIO.Demos.Store.Ports.ClientMobileBFF.Services.Interfaces;

public interface IGeneralGatewayService
{
    Task PingAsync(CancellationToken cancellationToken);
}
