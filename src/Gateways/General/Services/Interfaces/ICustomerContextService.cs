namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface ICustomerContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
