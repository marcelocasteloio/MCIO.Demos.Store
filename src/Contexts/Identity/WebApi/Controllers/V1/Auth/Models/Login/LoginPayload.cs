namespace MCIO.Demos.Store.Identity.WebApi.Controllers.V1.Auth.Models.Login;

public readonly record struct LoginPayload
(
    Guid TenantId,
    string Email,
    string Password,
    bool KeepConnected
);
