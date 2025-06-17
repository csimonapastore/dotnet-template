namespace BasicDotnetTemplate.MainProject.Models.Api.Data.User;

public class CreateUserRequestData : UpdateUserRequestData
{
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string? RoleGuid { get; set; }

}




