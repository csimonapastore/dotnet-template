using System.ComponentModel.DataAnnotations;

namespace BasicDotnetTemplate.MainProject.Models.Api.Data.Role;

public class CreateRoleRequestData
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }
    public required bool IsNotEditable { get; set; }

}
