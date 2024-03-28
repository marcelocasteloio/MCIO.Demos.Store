using MCIO.Core.ExecutionInfo;
using MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;
using MCIO.Demos.Store.BuildingBlock.WebApi.Responses;
using MCIO.Observability.Abstractions;
using MCIO.OutputEnvelop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MCIO.Demos.Store.BuildingBlock.WebApi.Controllers;
public class CustomControllerBase
    : ControllerBase
{
    // Constants
    public const string UNEXPECTED_ERROR_MESSAGE_CODE = "CustomControllerBase.UnexpectedError";
    public const int UNEXPECTED_ERROR_STATUS_CODE = 500;

    // Fields
    private readonly ITraceManager _traceManager;

    // Properties
    protected ILogger Logger { get; }
    protected IExecutionInfoAccessor ExecutionInfoAccessor { get; }

    // Constructors
    protected CustomControllerBase(
        ILogger logger,
        ITraceManager traceManager,
        IExecutionInfoAccessor executionInfoAccessor
    )
    {
        _traceManager = traceManager;

        Logger = logger;
        ExecutionInfoAccessor = executionInfoAccessor;
    }

    // Protected Methods
    protected async Task<IActionResult> ExecuteRequestAsync(
        Func<ExecutionInfo, Activity, CancellationToken, Task<OutputEnvelop.OutputEnvelop>> handler,
        int successStatusCode,
        int failStatusCode,
        CancellationToken cancellationToken,
        [CallerMemberName] string? caller = null
    )
    {
        try
        {
            var executionInfo = ExecutionInfoAccessor.CreateRequiredExecutionInfo();

            return await _traceManager.StartServerActivityAsync(
                name: caller,
                executionInfo: executionInfo,
                handler: async (activity, executionInfo, cancellationToken) =>
                {
                    var outputEnvelop = await handler(executionInfo, activity, cancellationToken);

                    return StatusCode(
                        statusCode: outputEnvelop.IsSuccess
                            ? successStatusCode
                            : failStatusCode,
                        value: ResponseBase.FromOutputEnvelop(outputEnvelop)
                    );
                },
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, UNEXPECTED_ERROR_MESSAGE_CODE);

            return StatusCode(
                statusCode: UNEXPECTED_ERROR_STATUS_CODE,
                value: OutputEnvelop.OutputEnvelop.CreateError(
                    outputMessageCode: UNEXPECTED_ERROR_MESSAGE_CODE,
                    outputMessageDescription: ex.Message
                )
            );
        }
    }
    protected async Task<IActionResult> ExecuteRequestAsync<THandlerOutput>(
        Func<ExecutionInfo, Activity, CancellationToken, Task<OutputEnvelop<THandlerOutput?>>> handler,
        int successStatusCode,
        int failStatusCode,
        CancellationToken cancellationToken,
        [CallerMemberName] string? caller = null
    )
    {
        try
        {
            var executionInfo = ExecutionInfoAccessor.CreateRequiredExecutionInfo();

            return await _traceManager.StartServerActivityAsync(
                name: caller,
                executionInfo: executionInfo,
                handler: async (activity, executionInfo, cancellationToken) =>
                {
                    var outputEnvelop = await handler(executionInfo, activity, cancellationToken);

                    return StatusCode(
                        statusCode: outputEnvelop.IsSuccess 
                            ? successStatusCode 
                            : failStatusCode,
                        value: ResponseBase.FromOutputEnvelop(outputEnvelop)
                    );
                },
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, UNEXPECTED_ERROR_MESSAGE_CODE);

            return StatusCode(
                statusCode: UNEXPECTED_ERROR_STATUS_CODE,
                value: OutputEnvelop<THandlerOutput>.CreateError(
                    output: default!,
                    outputMessageCode: UNEXPECTED_ERROR_MESSAGE_CODE,
                    outputMessageDescription: ex.Message
                )
            );
        }
    }
}
