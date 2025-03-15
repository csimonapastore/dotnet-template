using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BasicDotnetTemplate.MainProject.Core.Attributes;
using BasicDotnetTemplate.MainProject.Services;
//using BasicDotnetTemplate.MainProject.Models.Api.Request.User;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using BasicDotnetTemplate.MainProject.Models.Api.Response.User;

namespace BasicDotnetTemplate.MainProject.Controllers
{
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(
            IConfiguration configuration,
            IUserService userService
        ) : base(configuration)
        {
            this._userService = userService;
        }

        [JwtAuthorization()]
        [HttpGet("get/{guid}")]
        [ProducesResponseType<GetUserResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByGuidAsync(string guid)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Request is not well formed");
                }

                if (String.IsNullOrEmpty(guid))
                {
                    return BadRequest("Request is not well formed");
                }
                var data = await this._userService.GetUserByGuidAsync(guid);

                if (data == null || String.IsNullOrEmpty(data.Guid))
                {
                    return NotFound();
                }

                return Success(String.Empty, data);
            }
            catch (Exception exception)
            {
                var message = "Something went wrong";
                if (!String.IsNullOrEmpty(exception.Message))
                {
                    message += $". {exception.Message}";
                }
                return InternalServerError(message);
            }

        }
    }
}