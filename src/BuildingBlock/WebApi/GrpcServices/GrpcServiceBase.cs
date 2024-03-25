using MCIO.Core.ExecutionInfo;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using System.Diagnostics;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.GrpcServices;
public class GrpcServiceBase
{
    public static async Task<OutputEnvelop<TOutput?>> ProcessRequestAsync<TInput, TOutput>(
        ITraceManager traceManager,
        string traceName,
        ExecutionInfo executionInfo,
        TInput input,
        Func<Activity, ExecutionInfo, TInput, CancellationToken, Task<OutputEnvelop<TOutput>>> handler,
        CancellationToken cancellationToken
    )
    {
        return await traceManager.StartServerActivityAsync(
            name: traceName,
            executionInfo,
            input,
            handler: async (activity, executionInfo, input, cancellationToken) =>
            {
                return await handler(activity, executionInfo, input, cancellationToken);
            },
            cancellationToken
        );
    }
}
