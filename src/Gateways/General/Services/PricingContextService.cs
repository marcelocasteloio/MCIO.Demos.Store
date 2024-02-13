using MCIO.Demos.Store.Gateways.General.Services.Interfaces;

namespace MCIO.Demos.Store.Gateways.General.Services;

public class PricingContextService
    : IPricingContextService
{
    // Fields
    private readonly HttpClient _httpClient;
    private readonly Config.Config _config;

    // Constructors
    public PricingContextService(
        HttpClient httpClient,
        Config.Config config
    )
    {
        _httpClient = httpClient;
        _config = config;
    }

    // Public Methods
    public async Task PingAsync(CancellationToken cancellationToken)
    {
        await _httpClient.GetAsync(
            requestUri: $"{_config.Services.HttpServiceCollection.PricingContext.BaseUrl}/api/v1/ping",
            cancellationToken
        );
    }
}