using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;

namespace BasicDotnetTemplate.MainProject.Models.Api.Common.Role;

public class UserRole
{
#nullable enable
    public string? Guid { get; set; }
    public string? Name { get; set; }
#nullable disable

    public UserRole() {}

    public UserRole(DatabaseSqlServer.Role role)
    {
        Guid = role.Guid;
        Name = role.Name;
    }
}
