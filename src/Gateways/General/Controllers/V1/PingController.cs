using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class PingController 
    : ControllerBase
{
    // Constructors
    public PingController()
    {

    }

    [HttpGet]
    public Task<IActionResult> PingAsync()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
}
