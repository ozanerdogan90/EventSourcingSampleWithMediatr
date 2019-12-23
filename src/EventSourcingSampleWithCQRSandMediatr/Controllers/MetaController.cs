using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingSampleWithCQRSandMediatr.Controllers
{

    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    public class MetaController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger/");
        }

        [HttpGet]
        [Route("liveness")]
        public IActionResult Liveness()
        {
            return Ok("I am alive");
        }
    }
}
