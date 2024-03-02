using Asp.Versioning;
using MCIO.Demos.Store.Identity.WebApi.Controllers.V1.Auth.Models.Login;
using MCIO.Demos.Store.Identity.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Pipes;

namespace MCIO.Demos.Store.Identity.WebApi.Controllers.V1.Auth;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class AuthController
    : ControllerBase
{
    // Fields
    private ITokenService _tokenService;

    // Constructors
    public AuthController(
        ITokenService tokenService
    )
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginPayload payload,
        CancellationToken cancellationToken
    )
    {
        await Task.Yield();

        return Ok(
            _tokenService.Generate(
                new WebApi.Models.User(
                    Email: payload.Email,
                    Password: payload.Password,
                    Roles: [
                        "can-add-customer",    
                        "can-edit-customer",
                        "can-remove-customer"
                    ]
                )
            )
        );
    }
}
