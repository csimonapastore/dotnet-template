using System.ComponentModel.DataAnnotations;

namespace BasicDotnetTemplate.MainProject.Models.Database.SqlServer
{
    public class PermissionOperation : Base
    {
        [MaxLength(100)]
        public required string Name { get; set; }
    }
}
