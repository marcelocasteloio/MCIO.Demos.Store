﻿using MCIO.Demos.Store.Gateways.General.Services.Contexts.Base.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Contexts.Identity.V1.Models;
using MCIO.OutputEnvelop;

namespace MCIO.Demos.Store.Gateways.General.Services.Contexts.Identity.V1.Interfaces;

public interface IIdentityContextService
    : IContextService
{
    Task<OutputEnvelop<LoginResponse?>> LoginAsync(LoginPayload payload, CancellationToken cancellationToken);
    Task<OutputEnvelop<bool?>> ValidateTokenAsync(string authorizationHeaderValue, CancellationToken cancellationToken);
}