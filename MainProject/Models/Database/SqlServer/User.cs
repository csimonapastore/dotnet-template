using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BasicDotnetTemplate.MainProject.Models.Database.SqlServer;

public class User : Base
{
    [MaxLength(200)]
    public required string FirstName { get; set; }
    [MaxLength(200)]
    public required string LastName { get; set; }
    [MaxLength(200)]
    public required string Email { get; set; }
    public required string PasswordSalt { get; set; }
#nullable enable
    public string? PasswordPepper { get; set; }
#nullable disable
    public required int PasswordIterations { get; set; }
    public required Role Role { get; set; }
    public required bool IsTestUser { get; set; }

    [JsonIgnore]
    public required string Password { get; set; }
}




