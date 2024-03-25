namespace MCIO.Demos.Store.Ports.ClientWebBFF.Factories;

public static class ExecutionInfoFactory
{
    // Constants
    public const string CORRELATION_ID_SHOULD_BE_A_GUID_MESSAGE_CODE = "ExecutionInfoAdapter.CorrelationId.Should.Be.Guid";
    public const string CORRELATION_ID_SHOULD_BE_A_GUID_MESSAGE_DESCRIPTION = "CorrelationId shoul be a GUID";

    // Public Methods
    public static MCIO.Core.ExecutionInfo.ExecutionInfo? Create(Commom.Protos.V1.ExecutionInfo? executionInfo)
    {
        if (executionInfo == null)
            return null;

        if (!Guid.TryParse(executionInfo.CorrelationId, out var correlationId))
            throw new InvalidOperationException();

        if (!Guid.TryParse(executionInfo.TenantCode, out var tenantCode))
            throw new InvalidOperationException(
                $"{CORRELATION_ID_SHOULD_BE_A_GUID_MESSAGE_CODE}: {CORRELATION_ID_SHOULD_BE_A_GUID_MESSAGE_DESCRIPTION}"
            );

        var tenantInfoOutput = MCIO.Core.TenantInfo.TenantInfo.FromExistingCode(tenantCode);

        if (!tenantInfoOutput.IsSuccess)
            throw new InvalidOperationException(
                string.Join("|", tenantInfoOutput.OutputMessageCollection.Select(q => q.ToString()))
            );

        var executionInfoOutput = MCIO.Core.ExecutionInfo.ExecutionInfo.Create(
            correlationId,
            tenantInfo: tenantInfoOutput.Output!.Value,
            executionUser: executionInfo.User,
            origin: executionInfo.Origin
        );

        if (!executionInfoOutput.IsSuccess)
            throw new InvalidOperationException(
                string.Join("|", executionInfoOutput.OutputMessageCollection.Select(q => q.ToString()))
            );

        return executionInfoOutput.Output!.Value;
    }
    public static Commom.Protos.V1.ExecutionInfo Create(MCIO.Core.ExecutionInfo.ExecutionInfo executionInfo)
    {
        return new Commom.Protos.V1.ExecutionInfo
        {
            CorrelationId = executionInfo.CorrelationId.ToString(),
            TenantCode = executionInfo.TenantInfo.Code.ToString(),
            Origin = executionInfo.Origin,
            User = executionInfo.ExecutionUser
        };
    }
}
