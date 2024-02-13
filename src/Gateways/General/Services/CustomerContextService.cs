using MCIO.Demos.Store.Gateways.General.Services.Interfaces;

namespace MCIO.Demos.Store.Gateways.General.Services;

public class CustomerContextService
    : ICustomerContextService
{
    // Fields
    private readonly HttpClient _httpClient;
    private readonly Config.Config _config;

    // Constructors
    public CustomerContextService(
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
            requestUri: $"{_config.Services.HttpServiceCollection.CustomerContext.BaseUrl}/api/v1/ping",
            cancellationToken
        );
    }
}