using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BasicDotnetTemplate.MainProject.Core.Attributes;
using BasicDotnetTemplate.MainProject.Models.Api.Request.Auth;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using BasicDotnetTemplate.MainProject.Services;

namespace BasicDotnetTemplate.MainProject.Controllers
{
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private IAuthService _authService;
        public AuthController(
            IConfiguration configuration,
            IAuthService authService
        ) : base(configuration)
        {
            this._authService = authService;
        }

        [HttpPost("authenticate")]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType<BaseResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<BaseResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BaseResponse>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateRequest request)
        {
            try
            {
                if (
                    request == null ||
                    request.Data == null ||
                    String.IsNullOrEmpty(request.Data.Username) ||
                    String.IsNullOrEmpty(request.Data.Password)
                )
                {
                    return BadRequest("Request is not well formed");
                }
                var data = await this._authService.AuthenticateAsync(request.Data);

                if (data == null)
                {
                    return NotFound();
                }

                return Success(String.Empty, data);
            }
            catch (Exception exception)
            {
                var message = "Something went wrong";
                if (!String.IsNullOrEmpty(exception?.Message))
                {
                    message += $". {exception?.Message}";
                }
                return InternalServerError(message);
            }

        }
    }
}