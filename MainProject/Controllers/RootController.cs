using Microsoft.AspNetCore.Mvc;
using BasicDotnetTemplate.MainProject.Models.Settings;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BasicDotnetTemplate.MainProject.Controllers
{
    [Route("")]
    public class RootController : ControllerBase
    {
        public RootController() { }

        [HttpGet("")]
        public IActionResult GetRoot()
        {
            return Ok();
        }
    }
}