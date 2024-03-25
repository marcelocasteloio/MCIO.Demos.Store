using Asp.Versioning;
using MCIO.Demos.Store.BuildingBlock.WebApi.Controllers;
using MCIO.Demos.Store.BuildingBlock.WebApi.ExecutionInfoAccessor.Interfaces;
using MCIO.Demos.Store.BuildingBlock.WebApi.Responses;
using MCIO.Demos.Store.Ports.ClientMobileBFF.Services.Interfaces;
using MCIO.Observability.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Ports.ClientMobileBFF.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class PingController
    : CustomControllerBase
{
    // Fields
    private readonly IGeneralGatewayService _generalGatewayService;

    // Constructors
    public PingController(
        ILogger<PingController> logger,
        ITraceManager traceManager,
        IExecutionInfoAccessor executionInfoAccessor,
        IGeneralGatewayService generalGatewayService
    ) : base(logger, traceManager, executionInfoAccessor)
    {
        _generalGatewayService = generalGatewayService;
    }

    [HttpGet]
    [ProducesResponseType(type: typeof(ResponseBase), statusCode: 200)]
    [ProducesResponseType(type: typeof(ResponseBase), statusCode: 422)]
    [ProducesResponseType(type: typeof(ResponseBase), statusCode: 500)]
    [AllowAnonymous]
    public Task<IActionResult> PingAsync(CancellationToken cancellationToken)
    {
        return ProcessRequestAsync(
            handler: (executionInfo, activity, cancellationToken) =>
            {
                return _generalGatewayService.PingHttpAsync(executionInfo, cancellationToken);
            },
            successStatusCode: 200,
            failStatusCode: 422,
            cancellationToken
        );
    }
}
