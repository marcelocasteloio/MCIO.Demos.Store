using MCIO.Demos.Store.Gateways.General.Services.Identity.V1.Interfaces;
using MCIO.Demos.Store.Gateways.General.Services.Identity.V1.Models;
using MCIO.OutputEnvelop;
using MCIO.OutputEnvelop.Enums;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MCIO.Demos.Store.Gateways.General.Services.Identity.V1;

public class IdentityContextService
    : IIdentityContextService
{
    // Fields
    private readonly HttpClient _httpClient;
    private readonly Config.Config _config;

    // Constructors
    public IdentityContextService(
        HttpClient httpClient,
        Config.Config config
    )
    {
        _httpClient = httpClient;
        _config = config;
    }

    // Public Methods
    public async Task<OutputEnvelop<LoginResponse?>> LoginAsync(
        LoginPayload payload,
        CancellationToken cancellationToken
    )
    {
        var response = await _httpClient.PostAsJsonAsync(
            requestUri: $"{_config.Services.HttpServiceCollection.IdentityContext.BaseUrl}/api/v1/auth/login",
            value: payload,
            cancellationToken
        );

        return OutputEnvelop<LoginResponse?>.Create(
            output: await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken),
            type: response.IsSuccessStatusCode ? OutputEnvelopType.Success : OutputEnvelopType.Error
        );
    }
    public async Task<OutputEnvelop<bool?>> ValidateTokenAsync(
        string authorizationHeaderValue,
        CancellationToken cancellationToken
    )
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);

        var response = await _httpClient.GetAsync(
            requestUri: $"{_config.Services.HttpServiceCollection.IdentityContext.BaseUrl}/api/v1/auth/validate-token",
            cancellationToken
        );

        return OutputEnvelop<bool?>.Create(
            output: response.IsSuccessStatusCode,
            type: response.IsSuccessStatusCode ? OutputEnvelopType.Success : OutputEnvelopType.Error
        );
    }
    public async Task PingAsync(CancellationToken cancellationToken)
    {
        await _httpClient.GetAsync(
            requestUri: $"{_config.Services.HttpServiceCollection.IdentityContext.BaseUrl}/api/v1/ping",
            cancellationToken
        );
    }
}