using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions;
using MCIO.Demos.Store.BuildingBlock.WebApi.Responses;
using MCIO.Demos.Store.Commom.Protos.V1;
using MCIO.Demos.Store.Gateways.General.Factories;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base.Interfaces;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using System.Text.Json;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Base;

public abstract class ContextServiceBase
    : IContextService
{
    // Properties
    protected ITraceManager TraceManager { get; }
    protected HttpClient HttpClient { get; }
    protected Config.Config Config { get; }

    // Constructors
    protected ContextServiceBase(
        ITraceManager traceManager,
        HttpClient httpClient,
        Config.Config config
    )
    {
        TraceManager = traceManager;
        HttpClient = httpClient;
        Config = config;
    }

    // Public Methods
    public abstract Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
    public abstract Task<OutputEnvelop<PingReply?>> PingGrpcAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);

    // Protected Methods
    protected Task<OutputEnvelop.OutputEnvelop> ExecuteHttpRequestWithResponseBaseReturnAsync(
        string traceName,
        Core.ExecutionInfo.ExecutionInfo executionInfo,
        IResiliencePolicy resiliencePolicy,
        Func<CancellationToken, Task<HttpResponseMessage>> handler,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartInternalActivityAsync(
            name: traceName,
            executionInfo: executionInfo,
            handler: async (activity, executionInfo, cancellationToken) =>
            {
                return await resiliencePolicy.ExecuteAsync(
                    handler: async (cancellationToken) =>
                    {
                        var response = await handler(cancellationToken);

                        if (!response.IsSuccessStatusCode)
                            return OutputEnvelop.OutputEnvelop.CreateError();

                        var responseBase = JsonSerializer.Deserialize<ResponseBase>(await response.Content.ReadAsStringAsync(cancellationToken));

                        return responseBase?.ToOutputEnvelop((int)response.StatusCode) ?? OutputEnvelop.OutputEnvelop.CreateError();
                    },
                    cancellationToken
                );
            },
            cancellationToken
        );
    }
    protected Task<OutputEnvelop.OutputEnvelop<TOutput?>> ExecuteGrpcRequestWithReplyHeaderReturnAsync<TOutput>(
        string traceName,
        Core.ExecutionInfo.ExecutionInfo executionInfo,
        IResiliencePolicy resiliencePolicy,
        Func<CancellationToken, Task<(TOutput, ReplyHeader)>> handler,
        CancellationToken cancellationToken
    )
    {
        return TraceManager.StartInternalActivityAsync(
            name: traceName,
            executionInfo: executionInfo,
            handler: async (activity, executionInfo, cancellationToken) =>
            {
                return await resiliencePolicy.ExecuteAsync(
                    handler: async (cancellationToken) =>
                    {
                        var (output, replyHeader) = await handler(cancellationToken);

                        return replyHeader.ReplyResultType == ReplyResultType.Error
                            ? OutputEnvelop.OutputEnvelop<TOutput?>.CreateError(output)
                            : OutputEnvelopFactory.Create<TOutput?>(output, replyHeader);
                    },
                    cancellationToken
                );
            },
            cancellationToken
        );
    }
}
