namespace MCIO.Demos.Store.Gateways.General.Services.Customer.V1.Interfaces;

public interface ICustomerContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
