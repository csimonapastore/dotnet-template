using BasicDotnetTemplate.MainProject.Models.Api.Common.Role;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;

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

    public AuthenticatedUser(DatabaseSqlServer.User user)
    {
        Guid = user.Guid;
        Username = user.Username;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Role = new UserRole();
    }
}




