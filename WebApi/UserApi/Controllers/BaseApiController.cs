using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserApi.Controllers
{
    [Route("/")]
    [Produces("application/json")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
