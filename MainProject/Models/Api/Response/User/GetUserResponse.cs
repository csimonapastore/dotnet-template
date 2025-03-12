using BasicDotnetTemplate.MainProject.Models.Api.Common.User;

namespace BasicDotnetTemplate.MainProject.Models.Api.Response.User;

public class GetUserResponse : BaseResponse<UserDto>
{
    public GetUserResponse(int status, string? message, UserDto? data) : base(status, message, data) { }
}