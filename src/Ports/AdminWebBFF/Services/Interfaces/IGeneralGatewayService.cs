
namespace MCIO.Demos.Store.Ports.AdminWebBFF.Services.Interfaces;

public interface IGeneralGatewayService
{
    Task PingAsync(CancellationToken cancellationToken);
}
