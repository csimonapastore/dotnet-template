using System.ComponentModel.DataAnnotations;

namespace BasicDotnetTemplate.MainProject.Models.Database.SqlServer
{
    public class PermissionModule : Base
    {
        [MaxLength(100)]
        public required string Name { get; set; }
        public required bool Enabled { get; set; }
    }
}