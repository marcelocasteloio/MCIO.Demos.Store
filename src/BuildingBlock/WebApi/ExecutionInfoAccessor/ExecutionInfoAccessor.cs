using MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor;
public class ExecutionInfoAccessor
    : IExecutionInfoAccessor
{
    // Properties
    public Guid? TenantCode { get; }
    public string? User { get; }
    public string? AcceptLanguage { get; }

    // Constructors
    public ExecutionInfoAccessor(
        Guid? tenantCode,
        string? user,
        string? acceptLanguage
    )
    {
        TenantCode = tenantCode;
        User = user;
        AcceptLanguage = acceptLanguage;
    }
}
