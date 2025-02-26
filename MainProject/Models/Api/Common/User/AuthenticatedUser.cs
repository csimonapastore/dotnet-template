using BasicDotnetTemplate.MainProject.Models.Api.Common.Role;

namespace BasicDotnetTemplate.MainProject.Models.Api.Common.User;

public class AuthenticatedUser
{
#nullable enable
    public string? Guid { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public UserRole? Role { get; set; }
#nullable disable
}




