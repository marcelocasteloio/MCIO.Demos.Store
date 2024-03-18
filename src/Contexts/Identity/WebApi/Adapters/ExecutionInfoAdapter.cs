namespace MCIO.Demos.Store.Identity.WebApi.Adapters;

public static class ExecutionInfoAdapter
{
    public static Core.ExecutionInfo.ExecutionInfo? Adapt(Commom.Protos.V1.ExecutionInfo? executionInfo)
    {
        if (executionInfo == null)
            return null;

        if (!Guid.TryParse(executionInfo.CorrelationId, out var correlationId))
            throw new InvalidOperationException("CorrelationId shoul be a GUID");

        if (!Guid.TryParse(executionInfo.TenantCode, out var tenantCode))
            throw new InvalidOperationException("TenantCode shoul be a GUID");

        var tenantInfoOutput = Core.TenantInfo.TenantInfo.FromExistingCode(tenantCode);

        if (!tenantInfoOutput.IsSuccess)
            throw new InvalidOperationException(
                string.Join("|", tenantInfoOutput.OutputMessageCollection.Select(q => $"{q.Type} - {q.Code} - {q.Description}"))
            );

        var executionInfoOutput = Core.ExecutionInfo.ExecutionInfo.Create(
            correlationId,
            tenantInfo: tenantInfoOutput.Output!.Value,
            executionUser: executionInfo.User,
            origin: executionInfo.Origin
        );

        if (!executionInfoOutput.IsSuccess)
            throw new InvalidOperationException(
                string.Join("|", executionInfoOutput.OutputMessageCollection.Select(q => $"{q.Type} - {q.Code} - {q.Description}"))
            );

        return executionInfoOutput.Output!.Value;
    }
    public static Commom.Protos.V1.ExecutionInfo Adapt(Core.ExecutionInfo.ExecutionInfo executionInfo)
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
