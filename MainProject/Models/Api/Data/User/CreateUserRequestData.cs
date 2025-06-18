using System.ComponentModel.DataAnnotations;

namespace BasicDotnetTemplate.MainProject.Models.Api.Data.User;

public class CreateUserRequestData : UpdateUserRequestData
{
    [Required(ErrorMessage = "Email is required")]
    [StringLength(200, ErrorMessage = "Email's maxLength: 200")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }

    public string? RoleGuid { get; set; }

}




