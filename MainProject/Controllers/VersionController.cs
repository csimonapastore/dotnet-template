using Microsoft.AspNetCore.Mvc;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Controllers
{
    [Route("[controller]")]
    public class VersionController : BaseController
    {
        public VersionController(
            IConfiguration configuration
        ) : base(configuration)
        {

        }

        [HttpGet("get")]
        public IActionResult GetVersion()
        {
            return Success(String.Empty, _appSettings?.Settings?.Version);
        }
    }
}