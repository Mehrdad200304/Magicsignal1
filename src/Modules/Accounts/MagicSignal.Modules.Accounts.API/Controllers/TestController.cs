using Microsoft.AspNetCore.Mvc;

namespace MagicSignal.Modules.Accounts.API.Controllers
{
    [ApiController]
    [Route("api/accounts/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Pong from Accounts Module!");
        }
    }
}