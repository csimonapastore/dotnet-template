namespace BasicDotnetTemplate.MainProject.Models.Api.Data.Role;

public class CreateRoleRequestData
{
    public string Name { get; set; } = String.Empty;
    public required bool IsNotEditable { get; set; }

}