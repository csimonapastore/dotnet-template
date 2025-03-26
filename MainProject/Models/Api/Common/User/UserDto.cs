using BasicDotnetTemplate.MainProject.Models.Api.Common.Role;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;

namespace BasicDotnetTemplate.MainProject.Models.Api.Common.User;

public class UserDto
{
#nullable enable
    public string? Guid { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
#nullable disable
}




