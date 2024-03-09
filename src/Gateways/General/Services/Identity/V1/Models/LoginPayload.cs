namespace MCIO.Demos.Store.Gateways.General.Services.Identity.V1.Models;

public readonly record struct LoginPayload
(
    string Username,
    string Password,
    bool KeepConnected
);
