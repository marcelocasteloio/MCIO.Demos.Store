using MCIO.Demos.Store.Identity.WebApi.Models;
using MCIO.Demos.Store.Identity.WebApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MCIO.Demos.Store.Identity.WebApi.Services;

public class TokenService
    : ITokenService
{
    // Fields
    private readonly Config.Config _config;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly SigningCredentials _signingCredentials;

    // Constructors
    public TokenService(Config.Config config)
    {
        _config = config;

        Initialize(
            _config.PrivateKey,
            ref _jwtSecurityTokenHandler!,
            ref _signingCredentials!
        );
    }

    // Public Methods
    public string Generate(User user)
    {
        var securityTokenDescriptor = CreateSecurityTokenDescriptor(
            ttl: TimeSpan.FromSeconds(_config.ExpiresInSeconds),
            claimsIdentity: GenerateClaims(user)
        );

        var token = _jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

        return _jwtSecurityTokenHandler.WriteToken(token);
    }
    public static  ClaimsIdentity GenerateClaims(User user)
    {
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(type: ClaimTypes.Name, value: user.Email)); // User.Identity.Name

        foreach (var role in user.Roles)
            claimsIdentity.AddClaim(new Claim(type: ClaimTypes.Role, value: role)); // User.IsInRole [Authorize]

        claimsIdentity.AddClaim(new Claim(type: "sample-key", value: "sample-value"));

        return claimsIdentity;
    }

    // Private Methods
    private static void Initialize(
        string privateKey,
        ref JwtSecurityTokenHandler? jwtSecurityTokenHandler,
        ref SigningCredentials? signingCredentials
    )
    {
        jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(Encoding.ASCII.GetBytes(privateKey)),
            algorithm: SecurityAlgorithms.HmacSha256Signature
        );
    }
    private SecurityTokenDescriptor CreateSecurityTokenDescriptor(
        TimeSpan ttl,
        ClaimsIdentity claimsIdentity
    )
    {
        return new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            SigningCredentials = _signingCredentials,
            Expires = DateTime.UtcNow.Add(ttl),
        };
    }
}
