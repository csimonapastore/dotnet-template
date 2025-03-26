namespace BasicDotnetTemplate.MainProject.Models.Api.Data.User;

public class CreateUserRequestData
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string? RoleGuid { get; set; }

}




