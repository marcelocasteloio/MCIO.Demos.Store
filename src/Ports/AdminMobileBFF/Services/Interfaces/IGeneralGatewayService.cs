
namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Services.Interfaces;

public interface IGeneralGatewayService
{
    Task PingAsync(CancellationToken cancellationToken);
}
