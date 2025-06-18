using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BasicDotnetTemplate.MainProject.Core.Attributes;
using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Request.User;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using BasicDotnetTemplate.MainProject.Models.Api.Response.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using BasicDotnetTemplate.MainProject.Core.Filters;

namespace BasicDotnetTemplate.MainProject.Controllers
{
    [Route("[controller]")]
    public class UserController : BaseController
    {
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
        [ModelStateValidationHandledByFilterAttribute]
        [HttpGet("get/{guid}")]
        [ProducesResponseType<GetUserResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByGuidAsync(string guid)
        {
            try
            {
                var user = await this._userService.GetUserByGuidAsync(guid);

                if (user == null || String.IsNullOrEmpty(user.Guid))
                {
                    return NotFound();
                }

                var userDto = _mapper?.Map<UserDto>(user);

                return Success(String.Empty, userDto);
            }
            catch (Exception exception)
            {
                var message = this._somethingWentWrong;
                if (!String.IsNullOrEmpty(exception.Message))
                {
                    message += $". {exception.Message}";
                }
                return InternalServerError(message);
            }

        }

        // [JwtAuthorization()]
        [ModelStateValidationHandledByFilterAttribute]
        [HttpPost("create")]
        [ProducesResponseType<GetUserResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request) //NOSONAR
        {
            try
            {
                if (await this._userService.CheckIfEmailIsValid(request.Data.Email))
                {
                    var role = await this._roleService.GetRoleForUser(request.Data.RoleGuid);
                    if (role == null)
                    {
                        return BadRequest("Role not found");
                    }

                    var user = await this._userService.CreateUserAsync(request.Data, role);

                    if (user == null || String.IsNullOrEmpty(user.Guid))
                    {
                        return BadRequest("Not created");
                    }

                    var userDto = _mapper?.Map<UserDto>(user);

                    return Success(String.Empty, userDto);
                }
                else
                {
                    return BadRequest("Invalid email");
                }

            }
            catch (Exception exception)
            {
                var message = this._somethingWentWrong;
                if (!String.IsNullOrEmpty(exception.Message))
                {
                    message += $". {exception.Message}";
                }
                return InternalServerError(message);
            }

        }

        [JwtAuthorization()]
        [ModelStateValidationHandledByFilterAttribute]
        [HttpPut("update/{guid}")]
        [ProducesResponseType<GetUserResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRequest request, string guid) //NOSONAR
        {
            try
            {
                var user = await this._userService.GetUserByGuidAsync(guid);
                if(user == null)
                {
                    return NotFound();
                }

                user = await this._userService.UpdateUserAsync(request.Data, user);

                var userDto = _mapper?.Map<UserDto>(user);

                return Success(String.Empty, userDto);
                
            }
            catch (Exception exception)
            {
                var message = this._somethingWentWrong;
                if (!String.IsNullOrEmpty(exception.Message))
                {
                    message += $". {exception.Message}";
                }
                return InternalServerError(message);
            }

        }       

        [JwtAuthorization()]
        [ModelStateValidationHandledByFilterAttribute]
        [HttpPut("update/{guid}/password")]
        [ProducesResponseType<GetUserResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserPasswordAsync(string guid, string newPassword)
        {
            try
            {
                var user = await this._userService.GetUserByGuidAsync(guid);
                if(user == null)
                {
                    return NotFound();
                }

                user = await this._userService.UpdateUserPasswordAsync(user, newPassword);

                var userDto = _mapper?.Map<UserDto>(user);

                return Success(String.Empty, userDto);
                
            }
            catch (Exception exception)
            {
                var message = this._somethingWentWrong;
                if (!String.IsNullOrEmpty(exception.Message))
                {
                    message += $". {exception.Message}";
                }
                return InternalServerError(message);
            }

        } 

        [JwtAuthorization()]
        [ModelStateValidationHandledByFilterAttribute]
        [HttpPut("update/{guid}/role")]
        [ProducesResponseType<GetUserResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserRoleAsync(string guid, string roleGuid)
        {
            try
            {
                var role = await this._roleService.GetRoleForUser(roleGuid);
                if (role == null)
                {
                    return BadRequest("Role not found");
                }

                var user = await this._userService.GetUserByGuidAsync(guid);
                if(user == null)
                {
                    return NotFound();
                }

                user = await this._userService.UpdateUserRoleAsync(user, role);

                var userDto = _mapper?.Map<UserDto>(user);

                return Success(String.Empty, userDto);
                
            }
            catch (Exception exception)
            {
                var message = this._somethingWentWrong;
                if (!String.IsNullOrEmpty(exception.Message))
                {
                    message += $". {exception.Message}";
                }
                return InternalServerError(message);
            }

        }      
        
        [JwtAuthorization()]
        [ModelStateValidationHandledByFilterAttribute]
        [HttpDelete("{guid}")]
        [ProducesResponseType<GetUserResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BaseResponse<object>>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserByGuidAsync(string guid)
        {
            try
            {
                var user = await this._userService.GetUserByGuidAsync(guid);

                if (user == null || String.IsNullOrEmpty(user.Guid))
                {
                    return NotFound();
                }

                await this._userService.DeleteUserAsync(user);

                return Success(String.Empty);              
            }
            catch (Exception exception)
            {
                var message = this._somethingWentWrong;
                if (!String.IsNullOrEmpty(exception.Message))
                {
                    message += $". {exception.Message}";
                }
                return InternalServerError(message);
            }

        }

    }
}