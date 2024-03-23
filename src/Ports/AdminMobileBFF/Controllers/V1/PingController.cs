using Asp.Versioning;
using MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor;
using MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;
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
    private readonly IExecutionInfoAccessor _executionInfoAccessor;
    private readonly IGeneralGatewayService _generalGatewayService;

    // Constructors
    public PingController(
        IExecutionInfoAccessor executionInfoAccessor,
        IGeneralGatewayService generalGatewayService
    )
    {
        _executionInfoAccessor = executionInfoAccessor;
        _generalGatewayService = generalGatewayService;
    }

    [HttpGet]
    public async Task<IActionResult> PingAsync(
        CancellationToken cancellationToken
    )
    {
        var executionInfo = _executionInfoAccessor.CreateRequiredExecutionInfo();

        await _generalGatewayService.PingHttpAsync(
            executionInfo,
            cancellationToken
        );

        return Ok();
    }
}
