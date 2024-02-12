using Asp.Versioning;
using MCIO.Core.ExecutionInfo;
using MCIO.Core.TenantInfo;
using MCIO.Demos.Store.Ports.AdminMobileBFF.Config;
using MCIO.Observability.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Ports.AdminMobileBFF.Controllers.V1;

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
    private readonly HttpClient _httpClient;
    private readonly Config.Config _config;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        ITraceManager traceManager,
        HttpClient httpClient,
        Config.Config config
    )
    {
        _logger = logger;
        _traceManager = traceManager;
        _httpClient = httpClient;
        _config = config;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>?> GetAsync(CancellationToken cancellationToken)
    {
        return await _traceManager.StartInternalActivityAsync(
            name: "geração da previsão do tempo",
            executionInfo: ExecutionInfo.Create(Guid.NewGuid(), TenantInfo.FromExistingCode(Guid.NewGuid()).Output!.Value, "asd", "asd").Output!.Value,
            handler: async (activity, executionInfo, cancellationToken) =>
            {
                var requestUri = $"{_config.Services.HttpServiceCollection.GeneralGateway.BaseUrl}/api/v1/weather-forecast";

                return await _httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>(
                    requestUri, 
                    cancellationToken
                );
            },
            cancellationToken
        );
    }
}
