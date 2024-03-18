using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions.Enums;
using MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions.Models;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.BuildingBlock.Resilience.Abstractions;

public interface IResiliencePolicy
{
    // Properties
    string Name { get; }
    ResiliencePolicyOptions ResilienceConfig { get; }
    CircuitState CircuitState { get; }
    int CurrentRetryCount { get; }
    int CurrentCircuitBreakerOpenCount { get; }

    // Methods
    void CloseCircuitBreakerManually();
    void OpenCircuitBreakerManually();

    Task<OutputEnvelop.OutputEnvelop> ExecuteAsync(Func<CancellationToken, Task<OutputEnvelop.OutputEnvelop>> handler, CancellationToken cancellationToken);
    Task<OutputEnvelop<TOutput?>> ExecuteAsync<TOutput>(Func<CancellationToken, Task<OutputEnvelop<TOutput?>>> handler, CancellationToken cancellationToken);

    Task<OutputEnvelop.OutputEnvelop> ExecuteAsync<TInput>(Func<TInput?, CancellationToken, Task<OutputEnvelop.OutputEnvelop>> handler, TInput? input, CancellationToken cancellationToken);
    Task<OutputEnvelop<TOutput?>> ExecuteAsync<TInput, TOutput>(Func<TInput?, CancellationToken, Task<OutputEnvelop<TOutput?>>> handler, TInput? input, CancellationToken cancellationToken);
}
