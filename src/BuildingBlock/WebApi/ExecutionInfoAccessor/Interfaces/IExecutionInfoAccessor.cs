using MCIO.Core.ExecutionInfo;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;
public interface IExecutionInfoAccessor
{
    // Constants
    public const string TENANT_CODE_HEADER_KEY = "X-Tenant-Code";
    public const string CORRELATION_ID_HEADER_KEY = "X-Correlation-Id";
    public const string USER_HEADER_KEY = "X-User";
    public const string ACCEPT_LANGUAGE_HEADER_KEY = "Accept-Language";
    public const string ORIGIN_HEADER_KEY = "Origin";
    public const string HOST_HEADER_KEY = "Host";
    public const string DEFAULT_ORIGIN_VALUE = "Undefinied";

    public readonly static string FAIL_ON_CREATE_EXECUTION_INFO_MESSAGE_CODE = $"{nameof(IExecutionInfoAccessor)}.FailOnCreateExecutionInfo";
    public readonly static string FAIL_ON_CREATE_EXECUTION_INFO_MESSAGE_DESCRIPTION = $"{nameof(IExecutionInfoAccessor)} fail on create execution info";

    public readonly static string FAIL_ON_CREATE_TENANT_INFO_MESSAGE_CODE = $"{nameof(IExecutionInfoAccessor)}.FailOnCreateTenantInfo";
    public readonly static string FAIL_ON_CREATE_TENANT_INFO_MESSAGE_DESCRIPTION = $"{nameof(IExecutionInfoAccessor)} fail on create tenant info";

    public readonly static string CORRELATION_ID_IS_REQUIRED_MESSAGE_CODE = $"{nameof(IExecutionInfoAccessor)}.CorrelationId.IsRequired";
    public readonly static string CORRELATION_ID_IS_REQUIRED_MESSAGE_DESCRIPTION = $"{nameof(CorrelationId)} is required";

    public readonly static string TENANT_CODE_IS_REQUIRED_MESSAGE_CODE = $"{nameof(IExecutionInfoAccessor)}.TenantCode.IsRequired";
    public readonly static string TENANT_CODE_IS_REQUIRED_MESSAGE_DESCRIPTION = $"{nameof(TenantCode)} is required";

    // Properties
    Guid? TenantCode { get; }
    Guid? CorrelationId { get; }
    string? User { get; }
    string? AcceptLanguage { get; }
    string? Origin { get; }

    // Public Methods
    OutputEnvelop<ExecutionInfo?> CreateExecutionInfo();
    ExecutionInfo CreateRequiredExecutionInfo();
}
