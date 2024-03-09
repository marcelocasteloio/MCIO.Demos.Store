using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace MCIO.Demos.Store.Identity.WebApi.Controllers.V1.Users;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class UsersController 
    : ControllerBase
{

}
