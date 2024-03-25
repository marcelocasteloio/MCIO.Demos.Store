namespace MCIO.Demos.Store.Gateways.General.Services.Payment.V1.Interfaces;

public interface IPaymentContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
