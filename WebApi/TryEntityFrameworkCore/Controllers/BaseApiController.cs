using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("/")]
    [Produces("application/json")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
