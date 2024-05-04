using Microsoft.AspNetCore.Mvc;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Controllers
{
    [Route("")]
    public class RootController : BaseController
    {
        public RootController(
            IConfiguration configuration
        ) : base(configuration)
        {

        }

        [HttpGet("")]
        public IActionResult GetVersion()
        {
            return Success(String.Empty, "Success");
        }
    }
}