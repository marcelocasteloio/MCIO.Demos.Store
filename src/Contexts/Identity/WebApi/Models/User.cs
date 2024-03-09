namespace MCIO.Demos.Store.Identity.WebApi.Models;

public readonly record struct User
(
    string Email,
    string Password,
    string[] Roles
);
