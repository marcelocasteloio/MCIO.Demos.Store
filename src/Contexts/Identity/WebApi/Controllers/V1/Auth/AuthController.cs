using Asp.Versioning;
using MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;
using MCIO.Demos.Store.BuildingBlock.WebApi.Responses;
using MCIO.Demos.Store.Identity.WebApi.Controllers.V1.Auth.Models.Login;
using MCIO.Demos.Store.Identity.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Identity.WebApi.Controllers.V1.Auth;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class AuthController
    : ControllerBase
{
    // Fields
    private readonly ITokenService _tokenService;
    private readonly IExecutionInfoAccessor _executionInfoAccessor;

    // Constructors
    public AuthController(
        ITokenService tokenService,
        IExecutionInfoAccessor executionInfoAccessor
    )
    {
        _tokenService = tokenService;
        _executionInfoAccessor = executionInfoAccessor;
    }

    [HttpPost("login")]
    [ProducesResponseType(type: typeof(ResponseBase<LoginResponse>), statusCode: StatusCodes.Status200OK, contentType: "application/json")]
    [ProducesResponseType(type: typeof(ResponseBase), statusCode: StatusCodes.Status401Unauthorized, contentType: "application/json")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginPayload payload,
        CancellationToken cancellationToken
    )
    {
        await Task.Yield();

        return Ok(
            new LoginResponse(
                Token: _tokenService.Generate(
                    new WebApi.Models.User(
                        Email: payload.Username,
                        Password: payload.Password,
                        Roles: [
                            "can-add-customer",
                            "can-edit-customer",
                            "can-remove-customer"
                        ]
                    )
                ),
                Username: payload.Username
            )
        );
    }

    [HttpGet("validate-token")]
    [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status200OK)]
    [Authorize]
    public IActionResult ValidateToken()
    {
        return Ok(DateTime.UtcNow);
    }
}
