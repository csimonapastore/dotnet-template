using System.ComponentModel.DataAnnotations;

namespace BasicDotnetTemplate.MainProject.Models.Api.Data.User;

public class UpdateUserRequestData
{
    [Required(ErrorMessage = "FirstName is required")]
    [StringLength(200, ErrorMessage = "FirstName's maxLength: 200")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "LastName is required")]
    [StringLength(200, ErrorMessage = "LastName's maxLength: 200")]
    public required string LastName { get; set; }

}




