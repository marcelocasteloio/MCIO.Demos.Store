namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface IIdentityContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
