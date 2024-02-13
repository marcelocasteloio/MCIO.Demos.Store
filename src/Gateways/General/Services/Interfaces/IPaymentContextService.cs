namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface IPaymentContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
