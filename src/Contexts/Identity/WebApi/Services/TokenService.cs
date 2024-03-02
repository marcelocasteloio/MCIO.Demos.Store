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
            _config.Token.PrivateKey,
            ref _jwtSecurityTokenHandler!,
            ref _signingCredentials!
        );
    }

    // Public Methods
    public string Generate(User user)
    {
        var securityTokenDescriptor = CreateSecurityTokenDescriptor(
            ttl: TimeSpan.FromSeconds(_config.Token.ExpiresInSeconds),
            claimsIdentity: GenerateClaims(user)
        );

        var token = _jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

        return _jwtSecurityTokenHandler.WriteToken(token);
    }
    public ClaimsIdentity GenerateClaims(User user)
    {
        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(type: ClaimTypes.Name, value: user.Email)); // User.Identity.Name
        claimsIdentity.AddClaim(new Claim(type: JwtRegisteredClaimNames.Iss, value: _config.Token.Issuer)); // User.Identity.Name

        foreach (var role in user.Roles)
            claimsIdentity.AddClaim(new Claim(type: ClaimTypes.Role, value: role)); // User.IsInRole [Authorize]

        if(_config.Token.AudienceCollection is not null)
            foreach (var audience in _config.Token.AudienceCollection)
                claimsIdentity.AddClaim(new Claim(type: JwtRegisteredClaimNames.Aud, value: audience));

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
            key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)),
            algorithm: SecurityAlgorithms.HmacSha256Signature
        );
    }
    private SecurityTokenDescriptor CreateSecurityTokenDescriptor(
        TimeSpan ttl,
        ClaimsIdentity claimsIdentity
    )
    {
        var expires = DateTime.UtcNow.Add(ttl);

        return new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            SigningCredentials = _signingCredentials,
            Expires = expires
        };
    }
}
