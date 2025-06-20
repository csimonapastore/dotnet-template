using System.ComponentModel.DataAnnotations;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;

namespace BasicDotnetTemplate.MainProject.Models.Api.Request.User;

public class UpdateUserRequest
{
    [Required(ErrorMessage = "Data is required")]
    public required UpdateUserRequestData Data { get; set; }
}




