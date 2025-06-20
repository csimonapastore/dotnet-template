using System.ComponentModel.DataAnnotations;

namespace BasicDotnetTemplate.MainProject.Models.Database.SqlServer
{
    public class PermissionSystemModule : Base
    {
        public required int PermissionSystemId { get; set; }
        public required int PermissionModuleId { get; set; }
        public required PermissionSystem PermissionSystem { get; set; }
        public required PermissionModule PermissionModule { get; set; }
        public required bool Enabled { get; set; }
    }
}
