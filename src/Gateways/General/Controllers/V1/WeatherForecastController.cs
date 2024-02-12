using Asp.Versioning;
using MCIO.Core.ExecutionInfo;
using MCIO.Core.TenantInfo;
using MCIO.Observability.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Gateways.General.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class WeatherForecastController
    : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ITraceManager _traceManager;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        ITraceManager traceManager        
    )
    {
        _logger = logger;
        _traceManager = traceManager;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return _traceManager.StartInternalActivity(
            name: "geração da previsão do tempo",
            executionInfo: ExecutionInfo.Create(Guid.NewGuid(), TenantInfo.FromExistingCode(Guid.NewGuid()).Output!.Value, "asd", "asd").Output!.Value,
            handler: (activity, executionInfo) =>
            {
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                });
            }
        );
    }
}
