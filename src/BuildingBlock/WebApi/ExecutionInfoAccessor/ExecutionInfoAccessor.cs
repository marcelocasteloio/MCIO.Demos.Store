using MCIO.Core.TenantInfo;
using MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;
using MCIO.OutputEnvelop;
using System.Text;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor;
public class ExecutionInfoAccessor
    : IExecutionInfoAccessor
{
    // Fields
    private MCIO.Core.ExecutionInfo.ExecutionInfo? _executionInfo;

    // Properties
    public Guid? TenantCode { get; }
    public Guid? CorrelationId { get; }
    public string? User { get; }
    public string? AcceptLanguage { get; }
    public string? Origin { get; }

    // Constructors
    public ExecutionInfoAccessor(
        Guid? tenantCode,
        Guid? correlationId,
        string? user,
        string? acceptLanguage,
        string? origin
    )
    {
        TenantCode = tenantCode;
        CorrelationId = correlationId;
        User = user;
        AcceptLanguage = acceptLanguage;
        Origin = origin;
    }

    // Public Methods
    public OutputEnvelop<MCIO.Core.ExecutionInfo.ExecutionInfo?> CreateExecutionInfo()
    {
        if(_executionInfo is null)
        {
            if (CorrelationId is null)
                return OutputEnvelop<MCIO.Core.ExecutionInfo.ExecutionInfo?>.CreateError(
                    output: null,
                    outputMessageCode: IExecutionInfoAccessor.CORRELATION_ID_IS_REQUIRED_MESSAGE_CODE,
                    outputMessageDescription: IExecutionInfoAccessor.CORRELATION_ID_IS_REQUIRED_MESSAGE_DESCRIPTION
                );

            var createExecutionInfoOutput = MCIO.Core.ExecutionInfo.ExecutionInfo.Create(
                CorrelationId.Value,
                tenantInfo: default,
                executionUser: User,
                Origin
            );

            if (!createExecutionInfoOutput.IsSuccess)
                return OutputEnvelop.OutputEnvelop.CreateError(createExecutionInfoOutput);

            _executionInfo = createExecutionInfoOutput!.Output!;
        }

        return OutputEnvelop<MCIO.Core.ExecutionInfo.ExecutionInfo?>.CreateSuccess(_executionInfo!.Value);
    }

    public MCIO.Core.ExecutionInfo.ExecutionInfo CreateRequiredExecutionInfo()
    {
        if (_executionInfo is null)
        {
            if (CorrelationId is null)
                throw new InvalidOperationException($"{IExecutionInfoAccessor.CORRELATION_ID_IS_REQUIRED_MESSAGE_CODE}: {IExecutionInfoAccessor.CORRELATION_ID_IS_REQUIRED_MESSAGE_DESCRIPTION}");

            if (TenantCode is null)
                throw new InvalidOperationException($"{IExecutionInfoAccessor.TENANT_CODE_IS_REQUIRED_MESSAGE_CODE}: {IExecutionInfoAccessor.TENANT_CODE_IS_REQUIRED_MESSAGE_DESCRIPTION}");

            var tenantInfoOutput = TenantInfo.FromExistingCode(TenantCode.Value);

            if(!tenantInfoOutput.IsSuccess)
                throw new InvalidOperationException($"{IExecutionInfoAccessor.TENANT_CODE_IS_REQUIRED_MESSAGE_CODE}: {IExecutionInfoAccessor.TENANT_CODE_IS_REQUIRED_MESSAGE_DESCRIPTION}");

            var createExecutionInfoOutput = MCIO.Core.ExecutionInfo.ExecutionInfo.Create(
                CorrelationId.Value,
                tenantInfo: tenantInfoOutput.Output!.Value,
                executionUser: User,
                Origin
            );

            if (!createExecutionInfoOutput.IsSuccess)
                throw new InvalidOperationException($"{IExecutionInfoAccessor.FAIL_ON_CREATE_EXECUTION_INFO_MESSAGE_CODE}: {IExecutionInfoAccessor.FAIL_ON_CREATE_EXECUTION_INFO_MESSAGE_DESCRIPTION}");

            _executionInfo = createExecutionInfoOutput!.Output!;
        }

        return _executionInfo!.Value;
    }
}
