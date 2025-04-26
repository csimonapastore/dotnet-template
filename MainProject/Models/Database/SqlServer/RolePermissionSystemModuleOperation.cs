using System.ComponentModel.DataAnnotations;

namespace BasicDotnetTemplate.MainProject.Models.Database.SqlServer
{
    public class RolePermissionSystemModuleOperation : Base
    {
        public required int RoleId { get; set; }
        public required int PermissionSystemModuleOperationId { get; set; }
        public required bool Active { get; set; }
        public required Role Role { get; set; }
        public required PermissionSystemModuleOperation PermissionSystemModuleOperation { get; set; }
    }
}