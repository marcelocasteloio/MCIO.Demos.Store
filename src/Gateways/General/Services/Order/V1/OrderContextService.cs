using MCIO.Demos.Store.Gateways.General.Services.Order.V1.Interfaces;

namespace MCIO.Demos.Store.Gateways.General.Services.Order.V1;

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
    public async Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken)
    {
        await _httpClient.GetAsync(
            requestUri: $"{_config.ExternalServices.HttpServiceCollection.OrderContext.BaseUrl}/api/v1/ping",
            cancellationToken
        );

        return OutputEnvelop.OutputEnvelop.CreateSuccess();
    }
}