using System.ComponentModel.DataAnnotations;
using BasicDotnetTemplate.MainProject.Models.Api.Data.Role;

namespace BasicDotnetTemplate.MainProject.Models.Api.Request.Role;

public class CreateRoleRequest
{
    [Required(ErrorMessage = "Data is required")]
    public required CreateRoleRequestData? Data { get; set; }
}
