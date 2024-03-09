using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Basket.WebApi.Config.Services.HttpServices;

public class HttpService
{
    [Required]
    public string BaseUrl { get; set; } = null!;
}
