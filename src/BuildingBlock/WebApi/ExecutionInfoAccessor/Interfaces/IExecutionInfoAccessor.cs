namespace MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;
public interface IExecutionInfoAccessor
{
    // Constants
    public const string TENANT_CODE_HEADER_KEY = "X-Tenant-Code";
    public const string USER_HEADER_KEY = "X-User";
    public const string ACCEPT_LANGUAGE_HEADER_KEY = "Accept-Language";

    // Properties
    Guid? TenantCode { get; }
    string? User { get; }
    string? AcceptLanguage { get; }
}
