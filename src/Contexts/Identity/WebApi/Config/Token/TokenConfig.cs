using System.ComponentModel.DataAnnotations;

namespace MCIO.Demos.Store.Identity.WebApi.Config.Token;

public class TokenConfig
{
    [Required]
    public string PrivateKey { get; set; } = null!;
    
    [Required]
    public int ExpiresInSeconds { get; set; }

    [Required]
    public string Issuer { get; set; } = null!;

    public string[]? AudienceCollection { get; set; }
}
