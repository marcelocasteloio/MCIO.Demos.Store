namespace MCIO.Demos.Store.Gateways.General.Services.Interfaces;

public interface ICalendarContextService
{
    Task PingAsync(CancellationToken cancellationToken);
}
