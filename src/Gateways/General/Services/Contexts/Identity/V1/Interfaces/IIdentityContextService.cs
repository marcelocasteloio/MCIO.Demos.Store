using MCIO.Demos.Store.Gateways.General.Services.Contexts.Identity.V1.Models;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Identity.V1.Interfaces;

public interface IIdentityContextService
{
    Task<OutputEnvelop<LoginResponse?>> LoginAsync(LoginPayload payload, CancellationToken cancellationToken);
    Task<OutputEnvelop<bool?>> ValidateTokenAsync(string authorizationHeaderValue, CancellationToken cancellationToken);
    Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken);
    Task<OutputEnvelop<Commom.Protos.V1.PingReply?>> PingGrpcAsync(Core.ExecutionInfo.ExecutionInfo executionInfo, CancellationToken cancellationToken);
}
