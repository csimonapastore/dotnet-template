using System.ComponentModel.DataAnnotations;

namespace BasicDotnetTemplate.MainProject.Models.Api.Data.Auth;

public class AuthenticateRequestData
{
    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}




