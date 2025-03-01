using BasicDotnetTemplate.MainProject.Models.Api.Data.Auth;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;

namespace BasicDotnetTemplate.MainProject.Models.Api.Response.Auth;

public class AuthenticateResponse: BaseResponse
{
    public AuthenticateResponse(int status, string? message, AuthenticatedUser? data) : base(status, message, data) {}
}