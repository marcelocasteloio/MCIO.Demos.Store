using MCIO.Demos.Store.Gateways.General.Services.Interfaces;

namespace MCIO.Demos.Store.Gateways.General.Services;

public class OrderContextService
    : IOrderContextService
{
    // Fields
    private readonly HttpClient _httpClient;
    private readonly Config.Config _config;

    // Constructors
    public OrderContextService(
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
            requestUri: $"{_config.ExternalServices.HttpServiceCollection.OrderContext.BaseUrl}/api/v1/ping",
            cancellationToken
        );
    }
}