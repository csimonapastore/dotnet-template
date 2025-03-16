using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BasicDotnetTemplate.MainProject.Core.Attributes;
using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Request.User;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using BasicDotnetTemplate.MainProject.Models.Api.Response.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;

namespace BasicDotnetTemplate.MainProject.Controllers
{
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UserController(
            IConfiguration configuration,
            IUserService userService,
            IRoleService roleService
        ) : base(configuration)
        {
            this._userService = userService;
            this._roleService = roleService;
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
                    return BadRequest(_requestNotWellFormed);
                }

                if (String.IsNullOrEmpty(guid))
                {
                    return BadRequest(_requestNotWellFormed);
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

        [JwtAuthorization()]
        [HttpPost("create")]
        [ProducesResponseType<GetUserResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(_requestNotWellFormed);
                }

                if (request == null || request.Data == null ||
                    String.IsNullOrEmpty(request.Data.FirstName) ||
                    String.IsNullOrEmpty(request.Data.LastName) ||
                    String.IsNullOrEmpty(request.Data.Email) ||
                    String.IsNullOrEmpty(request.Data.Password)
                )
                {
                    return BadRequest(_requestNotWellFormed);
                }

                if (await this._userService.CheckIfEmailIsValid(request.Data.Email))
                {
                    return BadRequest();
                }
                else
                {
                    Role? role = null; // TODO
                    var data = await this._userService.CreateUser(request.Data, role);

                    if (data == null || String.IsNullOrEmpty(data.Guid))
                    {
                        return BadRequest();
                    }

                    return Success(String.Empty, data);
                }

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