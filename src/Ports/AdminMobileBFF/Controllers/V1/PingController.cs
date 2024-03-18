using Asp.Versioning;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class PingController 
    : ControllerBase
{
    // Fields
    private readonly IGeneralGatewayService _generalGatewayService;

    // Constructors
    public PingController(
        IGeneralGatewayService generalGatewayService
    )
    {
        _generalGatewayService = generalGatewayService;
    }

    [HttpGet]
    public async Task<IActionResult> PingAsync(CancellationToken cancellationToken)
    {
        await _generalGatewayService.PingHttpAsync(cancellationToken);

        return Ok();
    }
}
