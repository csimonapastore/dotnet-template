using System.ComponentModel.DataAnnotations;
using BasicDotnetTemplate.MainProject.Models.Api.Data.Auth;

namespace BasicDotnetTemplate.MainProject.Models.Api.Request.Auth;

public class AuthenticateRequest
{
    [Required(ErrorMessage = "Data is required")]
    public required AuthenticateRequestData Data { get; set; }
}




