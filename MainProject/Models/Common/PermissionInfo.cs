namespace BasicDotnetTemplate.MainProject.Models.Common;

public class PermissionInfo
{
#nullable enable
    public string? System { get; set; }
    public List<RolePermissionModuleOperation>? RolePermissionModuleOperations {get; set; }
#nullable disable
}