namespace MCIO.Demos.Store.Gateways.General.Services.Calendar.V1.Interfaces;

public interface ICalendarContextService
{
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
}
