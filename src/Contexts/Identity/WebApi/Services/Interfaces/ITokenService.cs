using MCIO.Demos.Store.Identity.WebApi.Models;

namespace MCIO.Demos.Store.Identity.WebApi.Services.Interfaces;

public interface ITokenService
{
    string Generate(User user);
}
