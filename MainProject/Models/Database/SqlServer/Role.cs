using System.ComponentModel.DataAnnotations;

namespace BasicDotnetTemplate.MainProject.Models.Database.SqlServer
{
    public class Role : Base
    {
        [MaxLength(100)]
        public required string Name { get; set; }
        public required bool IsNotEditable { get; set; }
    }
}



