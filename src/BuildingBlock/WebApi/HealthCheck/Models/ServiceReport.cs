using MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck.Models.Enums;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.HealthCheck.Models;
public readonly struct ServiceReport
{
    // Properties
    public DateTimeOffset Date { get; }
    public ServiceStatus Status { get; }
    public string StatusDescription => Status.ToString();
    public IEnumerable<ServiceReportItem>? ServiceReportItemCollection { get; }

    // Constructors
    public ServiceReport(DateTimeOffset date, IEnumerable<ServiceReportItem>? serviceReportItemCollection)
    {
        Date = date;
        ServiceReportItemCollection = serviceReportItemCollection;

        if (serviceReportItemCollection is null || !serviceReportItemCollection.Any())
            Status = ServiceStatus.Healthy;
        else if (serviceReportItemCollection.Any(q => q.Status == ServiceStatus.Unhealthy))
            Status = ServiceStatus.Unhealthy;
        else if (serviceReportItemCollection.Any(q => q.Status == ServiceStatus.Partial))
            Status = ServiceStatus.Partial;
        else
            Status = ServiceStatus.Healthy;
    }
}

