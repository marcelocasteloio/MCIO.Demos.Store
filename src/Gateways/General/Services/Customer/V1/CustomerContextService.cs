using MCIO.Demos.Store.Gateways.General.Services.Customer.V1.Interfaces;

namespace MCIO.Demos.Store.Gateways.General.Services.Customer.V1;

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
    public async Task<OutputEnvelop.OutputEnvelop> PingHttpAsync(CancellationToken cancellationToken)
    {
        await _httpClient.GetAsync(
            requestUri: $"{_config.ExternalServices.HttpServiceCollection.CustomerContext.BaseUrl}/api/v1/ping",
            cancellationToken
        );

        return OutputEnvelop.OutputEnvelop.CreateSuccess();
    }
}