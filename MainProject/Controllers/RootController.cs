using Microsoft.AspNetCore.Mvc;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Controllers
{
    public class RootController : BaseController
    {
        public RootController(
            IConfiguration configuration
        ) : base(configuration)
        {

        }

        [HttpGet]
        [Route("")]
        public IActionResult GetVersion()
        {
            return Success(String.Empty, "Success");
        }
    }
}