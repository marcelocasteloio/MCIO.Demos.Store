using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions.Models;
using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.BuildingBlock.Resilience;
public abstract class ResiliencePolicyBase
    : IResiliencePolicy
{
    // Constants
    private const int EXCEPTIONS_ALLOWED_BEFORE_BREAKING = 1;
    private const string RETRY_POLICY_CONTEXT_INPUT_KEY = "input";
    private const string RETRY_POLICY_CONTEXT_OUTPUT_KEY = "output";

    public const string RETRY_POLICY_FAILED_MESSAGE_CODE = "ResiliencePolicyBase.Failed";
    public const string RETRY_POLICY_FAILED_MESSAGE_DESCRIPTION = "ResiliencePolicy failed";

    // Fields
    private AsyncRetryPolicy _asyncRetryPolicy = null!;
    private AsyncCircuitBreakerPolicy _asyncCircuitBreakerPolicy = null!;

    // Public Properties
    public string Name { get; private set; } = null!;
    public Abstractions.Enums.CircuitState CircuitState => GetCircuitState(_asyncCircuitBreakerPolicy.CircuitState);
    public int CurrentRetryCount { get; private set; }
    public int CurrentCircuitBreakerOpenCount { get; private set; }
    public ResiliencePolicyOptions ResilienceConfig { get; private set; }

    // Constructors
    protected ResiliencePolicyBase()
    {
        Initialize();
    }

    // Public Methods
    public void CloseCircuitBreakerManually()
    {
        _asyncCircuitBreakerPolicy.Reset();
    }
    public void OpenCircuitBreakerManually()
    {
        _asyncCircuitBreakerPolicy.Isolate();
    }

    public async Task<OutputEnvelop.OutputEnvelop> ExecuteAsync(Func<CancellationToken, Task<OutputEnvelop.OutputEnvelop>> handler, CancellationToken cancellationToken)
    {
        var policyResult = await _asyncCircuitBreakerPolicy.ExecuteAndCaptureAsync(
            async (context, cancellationToken) =>
            {
                context.Add(
                    key: RETRY_POLICY_CONTEXT_OUTPUT_KEY,
                    value: await _asyncRetryPolicy.ExecuteAsync(async () =>
                        await handler(
                            cancellationToken
                        ).ConfigureAwait(false)
                    ).ConfigureAwait(false)
                );
            },
            contextData: new Dictionary<string, object?>(capacity: 1),
            cancellationToken
        ).ConfigureAwait(false);

        var success = policyResult.Outcome == OutcomeType.Successful;

        if (!success)
            return OutputEnvelop.OutputEnvelop.CreateError(
                RETRY_POLICY_FAILED_MESSAGE_CODE,
                RETRY_POLICY_FAILED_MESSAGE_DESCRIPTION
            );

        ResetCurrentRetryCount();

        return (OutputEnvelop.OutputEnvelop)policyResult.Context[RETRY_POLICY_CONTEXT_OUTPUT_KEY];
    }
    public async Task<OutputEnvelop<TOutput?>> ExecuteAsync<TOutput>(Func<CancellationToken, Task<OutputEnvelop<TOutput?>>> handler, CancellationToken cancellationToken)
    {
        var policyResult = await _asyncCircuitBreakerPolicy.ExecuteAndCaptureAsync(
            async (context, cancellationToken) =>
            {
                context.Add(
                    key: RETRY_POLICY_CONTEXT_OUTPUT_KEY,
                    value: await _asyncRetryPolicy.ExecuteAsync(async () =>
                        await handler(
                            cancellationToken
                        ).ConfigureAwait(false)
                    ).ConfigureAwait(false)
                );
            },
            contextData: new Dictionary<string, object?>(capacity: 1),
            cancellationToken
        ).ConfigureAwait(false);

        var success = policyResult.Outcome == OutcomeType.Successful;

        if (!success)
            return OutputEnvelop<TOutput?>.CreateError(
                output: default,
                RETRY_POLICY_FAILED_MESSAGE_CODE,
                RETRY_POLICY_FAILED_MESSAGE_DESCRIPTION
            );

        ResetCurrentRetryCount();

        return (OutputEnvelop<TOutput?>)policyResult.Context[RETRY_POLICY_CONTEXT_OUTPUT_KEY];
    }
    public async Task<OutputEnvelop.OutputEnvelop> ExecuteAsync<TInput>(Func<TInput?, CancellationToken, Task<OutputEnvelop.OutputEnvelop>> handler, TInput? input, CancellationToken cancellationToken)
    {
        var policyResult = await _asyncCircuitBreakerPolicy.ExecuteAndCaptureAsync(
            async (context, cancellationToken) =>
            {
                context.Add(
                    key: RETRY_POLICY_CONTEXT_OUTPUT_KEY,
                    value: await _asyncRetryPolicy.ExecuteAsync(async () =>
                        await handler(
                            (TInput)context[RETRY_POLICY_CONTEXT_INPUT_KEY],
                            cancellationToken
                        ).ConfigureAwait(false)
                    ).ConfigureAwait(false)
                );
            },
            contextData: new Dictionary<string, object?> { { RETRY_POLICY_CONTEXT_INPUT_KEY, input } },
            cancellationToken
        ).ConfigureAwait(false);

        var success = policyResult.Outcome == OutcomeType.Successful;

        if (!success)
            return OutputEnvelop.OutputEnvelop.CreateError(
                RETRY_POLICY_FAILED_MESSAGE_CODE,
                RETRY_POLICY_FAILED_MESSAGE_DESCRIPTION
            );

        ResetCurrentRetryCount();

        return (OutputEnvelop.OutputEnvelop)policyResult.Context[RETRY_POLICY_CONTEXT_OUTPUT_KEY];
    }
    public async Task<OutputEnvelop<TOutput?>> ExecuteAsync<TInput, TOutput>(Func<TInput?, CancellationToken, Task<OutputEnvelop<TOutput?>>> handler, TInput? input, CancellationToken cancellationToken)
    {
        var policyResult = await _asyncCircuitBreakerPolicy.ExecuteAndCaptureAsync(
            async (context, cancellationToken) =>
            {
                context.Add(
                    key: RETRY_POLICY_CONTEXT_OUTPUT_KEY,
                    value: await _asyncRetryPolicy.ExecuteAsync(async () =>
                        await handler(
                            (TInput)context[RETRY_POLICY_CONTEXT_INPUT_KEY],
                            cancellationToken
                        ).ConfigureAwait(false)
                    ).ConfigureAwait(false)
                );
            },
            contextData: new Dictionary<string, object?> { { RETRY_POLICY_CONTEXT_INPUT_KEY, input } },
            cancellationToken
        ).ConfigureAwait(false);

        var success = policyResult.Outcome == OutcomeType.Successful;

        if (!success)
            return OutputEnvelop<TOutput?>.CreateError(
                output: default,
                RETRY_POLICY_FAILED_MESSAGE_CODE,
                RETRY_POLICY_FAILED_MESSAGE_DESCRIPTION
            );

        ResetCurrentRetryCount();

        return (OutputEnvelop<TOutput?>)policyResult.Context[RETRY_POLICY_CONTEXT_OUTPUT_KEY];
    }

    public override string ToString()
    {
        return $"Resilience Policy [{Name}]";
    }

    // Protected Methods
    protected abstract void ConfigureInternal(ResiliencePolicyOptions options);

    // Private Methods
    private void Initialize()
    {
        var resilienceConfig = new ResiliencePolicyOptions();

        ConfigureInternal(resilienceConfig);
        ApplyConfig(resilienceConfig);

        ResilienceConfig = resilienceConfig;
    }

    private void ResetCurrentRetryCount() => CurrentRetryCount = 0;
    private void IncrementRetryCount() => CurrentRetryCount++;
    private void ResetCurrentCircuitBreakerOpenCount() => CurrentCircuitBreakerOpenCount = 0;
    private void IncrementCircuitBreakerOpenCount() => CurrentCircuitBreakerOpenCount++;
    private void ConfigureRetryPolicy(ResiliencePolicyOptions resiliencePolicyConfig)
    {
        var retryPolicyBuilder = default(PolicyBuilder);

        foreach (var exceptionHandleConfig in resiliencePolicyConfig.ExceptionHandleConfigArray)
            retryPolicyBuilder = retryPolicyBuilder is null 
                ? Policy.Handle(exceptionHandleConfig) 
                : retryPolicyBuilder.Or(exceptionHandleConfig);

        _asyncRetryPolicy = retryPolicyBuilder.WaitAndRetryAsync(
            retryCount: resiliencePolicyConfig.RetryMaxAttemptCount,
            sleepDurationProvider: resiliencePolicyConfig.RetryAttemptWaitingTimeFunction,
            onRetry: (exception, retryAttemptWaitingTime) =>
            {
                IncrementRetryCount();

                resiliencePolicyConfig.OnRetryAditionalHandler?.Invoke((CurrentRetryCount, retryAttemptWaitingTime, exception));
            }
        );
    }
    private void ConfigureCircuitBreakerPolicy(ResiliencePolicyOptions resiliencePolicyConfig)
    {
        var circuitBreakerPolicyBuilder = default(PolicyBuilder);

        foreach (var exceptionHandleConfig in resiliencePolicyConfig.ExceptionHandleConfigArray)
            circuitBreakerPolicyBuilder = circuitBreakerPolicyBuilder is null
                ? Policy.Handle(exceptionHandleConfig)
                : circuitBreakerPolicyBuilder.Or(exceptionHandleConfig);

        _asyncCircuitBreakerPolicy = circuitBreakerPolicyBuilder.CircuitBreakerAsync(
            exceptionsAllowedBeforeBreaking: EXCEPTIONS_ALLOWED_BEFORE_BREAKING,
            durationOfBreak: resiliencePolicyConfig.CircuitBreakerWaitingTimeFunction(),
            onBreak: (exception, waitingTime) =>
            {
                IncrementCircuitBreakerOpenCount();

                resiliencePolicyConfig.OnCircuitBreakerOpenAditionalHandler?.Invoke((CurrentCircuitBreakerOpenCount, waitingTime, exception));
            },
            onReset: () =>
            {
                ResetCurrentRetryCount();
                ResetCurrentCircuitBreakerOpenCount();

                resiliencePolicyConfig.OnCircuitBreakerCloseAditionalHandler?.Invoke();
            },
            onHalfOpen: () =>
            {
                ResetCurrentRetryCount();

                resiliencePolicyConfig.OnCircuitBreakerHalfOpenAditionalHandler?.Invoke();
            }
        );
    }
    private void ApplyConfig(ResiliencePolicyOptions resiliencePolicyConfig)
    {
        Name = resiliencePolicyConfig.Name;

        ConfigureRetryPolicy(resiliencePolicyConfig);
        ConfigureCircuitBreakerPolicy(resiliencePolicyConfig);
    }
    private static Abstractions.Enums.CircuitState GetCircuitState(CircuitState pollyCircuitState) =>
        pollyCircuitState switch
        {
            Polly.CircuitBreaker.CircuitState.Closed => Abstractions.Enums.CircuitState.Closed,
            Polly.CircuitBreaker.CircuitState.Open => Abstractions.Enums.CircuitState.Open,
            Polly.CircuitBreaker.CircuitState.HalfOpen => Abstractions.Enums.CircuitState.HalfOpen,
            Polly.CircuitBreaker.CircuitState.Isolated => Abstractions.Enums.CircuitState.Isolated,
            _ => throw new ArgumentOutOfRangeException(nameof(pollyCircuitState))
        };
}
