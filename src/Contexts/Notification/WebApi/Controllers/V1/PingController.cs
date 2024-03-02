using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Notification.WebApi.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class PingController
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> PingAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return Ok();
    }
}
