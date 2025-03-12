using System.Text.Json.Serialization;

namespace BasicDotnetTemplate.MainProject.Models.Database.SqlServer;

public class User : Base
{
    public required string Username { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PasswordSalt { get; set; }
    public required string PasswordHash { get; set; }
    public required Role Role { get; set; }
    public required bool IsTestUser { get; set; }

    [JsonIgnore]
    public required string Password { get; set; }
}




