using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthTestController : ControllerBase
    {
        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            return Ok("This is a secured endpoint!");
        }
    }
}
