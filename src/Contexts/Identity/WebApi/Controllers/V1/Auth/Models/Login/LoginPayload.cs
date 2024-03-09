namespace MCIO.Demos.Store.Identity.WebApi.Controllers.V1.Auth.Models.Login;

public readonly record struct LoginPayload
(
    string Username,
    string Password,
    bool KeepConnected
);
