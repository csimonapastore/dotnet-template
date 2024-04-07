using Microsoft.AspNetCore.Mvc;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Controllers
{
    public class VersionController : BaseController
    {
        public VersionController(
            IConfiguration configuration
        ) : base(configuration)
        {

        }

        [HttpGet]
        [Route("version")]
        public IActionResult GetVersion()
        {
            return Ok(new { version = _appSettings.Settings.Version });
        }
    }
}