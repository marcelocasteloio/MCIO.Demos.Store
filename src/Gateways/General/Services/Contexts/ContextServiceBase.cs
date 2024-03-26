using MCIO.Core.ExecutionInfo;
using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions;
using MCIO.Demos.Store.BuildingBlock.WebApi.Responses;
using MCIO.Observability.Abstractions;
using MCIO.Observability.OpenTelemetry;
using System.Text.Json;
using System.Threading;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts;

public abstract class ContextServiceBase
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

    // Protected Methods
    protected Task<OutputEnvelop.OutputEnvelop> ExecuteHttpRequestWithResponseBaseReturnAsync(
        string traceName,
        ExecutionInfo executionInfo,
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
}
