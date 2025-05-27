using System.ComponentModel.DataAnnotations;

namespace BasicDotnetTemplate.MainProject.Models.Database.SqlServer
{
    public class PermissionSystemModuleOperation : Base
    {
        public required int PermissionSystemModuleId { get; set; }
        public required int PermissionOperationId { get; set; }
        public required bool Enabled { get; set; }
        public required PermissionSystemModule PermissionSystemModule { get; set; }
        public required PermissionOperation PermissionOperation { get; set; }
    }
}