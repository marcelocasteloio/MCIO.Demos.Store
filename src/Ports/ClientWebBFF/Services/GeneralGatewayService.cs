using MCIO.Demos.Store.Ports.ClientWebBFF.Services.Interfaces;

namespace MCIO.Demos.Store.Ports.ClientWebBFF.Services;

public class GeneralGatewayService
    : IGeneralGatewayService
{
    // Fields
    private readonly HttpClient _httpClient;
    private readonly Config.Config _config;

    // Constructors
    public GeneralGatewayService(
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
            requestUri: $"{_config.Services.HttpServiceCollection.GeneralGateway.BaseUrl}/api/v1/ping", 
            cancellationToken
        );
    }
}
