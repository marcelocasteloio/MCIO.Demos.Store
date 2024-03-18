namespace MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions.Models;

public struct ResiliencePolicyOptions
{
    // Constants
    public const int DEFAULT_RETRY_MAX_ATTEMPT_COUNT = 3;
    public const int DEFAULT_RETRY_ATTEMPT_WAITING_TIME_IN_SECONDS = 1;
    public const int DEFAULT_CIRCUIT_BREAKER_WAITING_TIME_IN_SECONDS = 30;

    // Properties
    public string Name { get; private set; } = null!;
    public int RetryMaxAttemptCount { get; private set; }
    public Func<int, TimeSpan> RetryAttemptWaitingTimeFunction { get; private set; } = null!;
    public Action<(int CurrentRetryCount, TimeSpan RetryAttemptWaitingTime, Exception Exception)>? OnRetryAditionalHandler { get; private set; }
    public Func<TimeSpan> CircuitBreakerWaitingTimeFunction { get; private set; } = null!;
    public Action? OnCircuitBreakerHalfOpenAditionalHandler { get; private set; }
    public Action<(int CurrentCircuitBreakerOpenCount, TimeSpan CircuitBreakerWaitingTime, Exception Exception)>? OnCircuitBreakerOpenAditionalHandler { get; private set; }
    public Action? OnCircuitBreakerCloseAditionalHandler { get; private set; }
    public Func<Exception, bool>[] ExceptionHandleConfigArray { get; private set; } = null!;

    // Constructors
    public ResiliencePolicyOptions()
    {
        Name = Guid.NewGuid().ToString();

        RetryMaxAttemptCount = DEFAULT_RETRY_MAX_ATTEMPT_COUNT;
        RetryAttemptWaitingTimeFunction = attempt => TimeSpan.FromSeconds(DEFAULT_RETRY_ATTEMPT_WAITING_TIME_IN_SECONDS);

        CircuitBreakerWaitingTimeFunction = () => TimeSpan.FromSeconds(DEFAULT_CIRCUIT_BREAKER_WAITING_TIME_IN_SECONDS);

        ExceptionHandleConfigArray = [new Func<Exception, bool>(q => true)];
    }


    // Public Methods
    public ResiliencePolicyOptions WithCustomIdentificationOptions(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;

        return this;
    }

    public ResiliencePolicyOptions WithCustomRetryOptions(
        int? retryMaxAttemptCount = null,
        Func<int, TimeSpan>? retryAttemptWaitingTimeFunction = null,
        Action<(int CurrentRetryCount, TimeSpan RetryAttemptWaitingTime, Exception Exception)>? onRetryAditionalHandler = null
    )
    {
        if (retryMaxAttemptCount != null)
        {
            if (retryMaxAttemptCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(retryMaxAttemptCount));

            RetryMaxAttemptCount = retryMaxAttemptCount.Value;
        }

        if (retryAttemptWaitingTimeFunction != null)
            RetryAttemptWaitingTimeFunction = retryAttemptWaitingTimeFunction;

        if (onRetryAditionalHandler != null)
            OnRetryAditionalHandler = onRetryAditionalHandler;

        return this;
    }

    public ResiliencePolicyOptions WithCustomCircuitBreakerOptions(
        Func<TimeSpan>? circuitBreakerWaitingTimeFunction = null,
        Action? onCircuitBreakerHalfOpenAditionalHandler = null,
        Action<(int CurrentCircuitBreakerOpenCount, TimeSpan CircuitBreakerWaitingTime, Exception Exception)>? onCircuitBreakerOpenAditionalHandler = null,
        Action? onCircuitBreakerCloseAditionalHandler = null
    )
    {
        if (circuitBreakerWaitingTimeFunction != null)
            CircuitBreakerWaitingTimeFunction = circuitBreakerWaitingTimeFunction;

        if (onCircuitBreakerHalfOpenAditionalHandler != null)
            OnCircuitBreakerHalfOpenAditionalHandler = onCircuitBreakerHalfOpenAditionalHandler;

        if (onCircuitBreakerOpenAditionalHandler != null)
            OnCircuitBreakerOpenAditionalHandler = onCircuitBreakerOpenAditionalHandler;

        if (OnCircuitBreakerCloseAditionalHandler != null)
            OnCircuitBreakerCloseAditionalHandler = onCircuitBreakerCloseAditionalHandler;

        return this;
    }

    public ResiliencePolicyOptions WithCustomExceptionHandlerOptions(
        Func<Exception, bool>[]? exceptionHandleConfigArray
    )
    {
        if (exceptionHandleConfigArray != null)
            ExceptionHandleConfigArray = exceptionHandleConfigArray;

        return this;
    }
}
