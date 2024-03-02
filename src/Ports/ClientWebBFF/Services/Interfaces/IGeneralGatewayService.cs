
namespace MCIO.Demos.Store.Ports.ClientWebBFF.Services.Interfaces;

public interface IGeneralGatewayService
{
    Task PingAsync(CancellationToken cancellationToken);
}
