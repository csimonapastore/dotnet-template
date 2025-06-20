using System.ComponentModel.DataAnnotations;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;

namespace BasicDotnetTemplate.MainProject.Models.Api.Request.User;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Data is required")]
    public required CreateUserRequestData Data { get; set; }
}




