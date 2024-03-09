using MCIO.Demos.Store.Gateways.General.Services.Identity.V1.Models;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Gateways.General.Services.Identity.V1.Interfaces;

public interface IIdentityContextService
{
    Task<OutputEnvelop<LoginResponse?>> LoginAsync(LoginPayload payload, CancellationToken cancellationToken);
    Task PingAsync(CancellationToken cancellationToken);
    Task<OutputEnvelop<bool?>> ValidateTokenAsync(string authorizationHeaderValue, CancellationToken cancellationToken);
}
