namespace BasicDotnetTemplate.MainProject.Models.Common;

public class RolePermissionModuleOperation
{
#nullable enable
    public string? Module { get; set; }
    public List<OperationInfo>? Operations { get; set; }
#nullable disable
}
