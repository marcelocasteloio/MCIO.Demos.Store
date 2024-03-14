using Asp.Versioning;
using MCIO.Demos.Store.Gateways.General.Services.Identity.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Identity.V1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Gateways.General.Controllers.V1.Auth;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class AuthController 
    : ControllerBase
{
    // Fields
    private readonly IIdentityContextService _identityContextService;

    // Constructors
    public AuthController(
        IIdentityContextService identityContextService
    )
    {
        _identityContextService = identityContextService;
    }

    [HttpPost("login")]
    [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginPayload payload,
        CancellationToken cancellationToken
    )
    {
        var loginResponseOutputEnvelop = await _identityContextService.LoginAsync(payload, cancellationToken);

        return loginResponseOutputEnvelop.IsSuccess
            ? Ok(loginResponseOutputEnvelop.Output) 
            : BadRequest();
    }

    [HttpGet("validate-token")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> ValidateTokenAsync(
        CancellationToken cancellationToken
    )
    {
        var result = await _identityContextService.ValidateTokenAsync(
            authorizationHeaderValue: HttpContext.Request.Headers.Authorization!,
            cancellationToken
        );

        return result.IsSuccess
            ? Ok() 
            : BadRequest();
    }
}
