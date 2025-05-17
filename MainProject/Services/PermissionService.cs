
using System.Collections;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Models.Api.Common.Exceptions;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Microsoft.EntityFrameworkCore;
using BasicDotnetTemplate.MainProject.Models.Common;
using BasicDotnetTemplate.MainProject.Utils;

namespace BasicDotnetTemplate.MainProject.Services;

public interface IPermissionService
{
    Task<PermissionSystem?> GetPermissionSystemByGuidAsync(string guid);
    Task<PermissionSystem?> GetPermissionSystemByNameAsync(string name);
    Task<bool?> HandleEnabledPermissionSystemAsync(PermissionSystem permission, bool enabled);
    Task<PermissionSystem?> CreatePermissionSystemAsync(string name, bool enabled);
    Task<bool?> DeletePermissionSystemAsync(PermissionSystem permission);


    Task<PermissionModule?> GetPermissionModuleByGuidAsync(string guid);
    Task<PermissionModule?> GetPermissionModuleByNameAsync(string name);
    Task<bool?> HandleEnabledPermissionModuleAsync(PermissionModule permission, bool enabled);
    Task<PermissionModule?> CreatePermissionModuleAsync(string name, bool enabled);
    Task<bool?> DeletePermissionModuleAsync(PermissionModule permission);


    Task<PermissionOperation?> GetPermissionOperationByGuidAsync(string guid);
    Task<PermissionOperation?> GetPermissionOperationByNameAsync(string name);
    Task<PermissionOperation?> CreatePermissionOperationAsync(string name);
    Task<bool?> DeletePermissionOperationAsync(PermissionOperation permission);


    Task<PermissionSystemModule?> GetPermissionSystemModuleByGuidAsync(string guid);
    Task<bool?> HandleEnabledPermissionSystemModuleAsync(PermissionSystemModule permission, bool enabled);
    Task<PermissionSystemModule?> CreatePermissionSystemModuleAsync(
        PermissionSystem permissionSystem,
        PermissionModule permissionModule,
        bool enabled
    );
    Task<bool?> DeletePermissionSystemModuleAsync(PermissionSystemModule permission);


    Task<PermissionSystemModuleOperation?> GetPermissionSystemModuleOperationByGuidAsync(string guid);
    Task<bool?> HandleEnabledPermissionSystemModuleOperationAsync(PermissionSystemModuleOperation permission, bool enabled);
    Task<PermissionSystemModuleOperation?> CreatePermissionSystemModuleOperationAsync(
        PermissionSystemModule permissionSystemModule,
        PermissionOperation permissionOperation,
        bool enabled
    );
    Task<bool?> DeletePermissionSystemModuleOperationAsync(PermissionSystemModuleOperation permission);


    Task<RolePermissionSystemModuleOperation?> GetRolePermissionSystemModuleOperationByGuidAsync(string guid);
    Task<bool?> HandleEnabledRolePermissionSystemModuleOperationAsync(RolePermissionSystemModuleOperation permission, bool active);
    Task<RolePermissionSystemModuleOperation?> CreateRolePermissionSystemModuleOperationAsync(
        Role role,
        PermissionSystemModuleOperation permissionSystemModuleOperation,
        bool enabled
    );
    Task<bool?> DeleteRolePermissionSystemModuleOperationAsync(RolePermissionSystemModuleOperation permission);

    List<string> CreatePermissionsOnStartupAsync();

}

public class PermissionService : BaseService, IPermissionService
{
    private readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    private readonly CommonDbMethodsUtils _commonDbMethodsUtils;

    public PermissionService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        SqlServerContext sqlServerContext
    ) : base(httpContextAccessor, configuration, sqlServerContext)
    {
        _commonDbMethodsUtils = new CommonDbMethodsUtils(sqlServerContext);
    }

    private IQueryable<Role> GetRoleByNameQueryable(string name)
    {
        return _commonDbMethodsUtils.GetRoleByNameQueryable(name);
    }

    private IQueryable<PermissionSystem> GetPermissionSystemsQueryable()
    {
        return this._sqlServerContext.PermissionSystems
            .Where(x => !x.IsDeleted);
    }

    private IQueryable<PermissionModule> GetPermissionModulesQueryable()
    {
        return this._sqlServerContext.PermissionModules
            .Where(x => !x.IsDeleted);
    }

    private IQueryable<PermissionOperation> GetPermissionOperationsQueryable()
    {
        return this._sqlServerContext.PermissionOperations
            .Where(x => !x.IsDeleted);
    }

    private IQueryable<PermissionSystemModule> GetPermissionSystemModulesQueryable()
    {
        return this._sqlServerContext.PermissionSystemModules
            .Where(x => !x.IsDeleted);
    }

    private IQueryable<PermissionSystemModuleOperation> GetPermissionSystemModuleOperationsQueryable()
    {
        return this._sqlServerContext.PermissionSystemModuleOperations
            .Include(x => x.PermissionOperation)
            .Include(x => x.PermissionSystemModule)
                .ThenInclude(x => x.PermissionSystem)
            .Where(x => !x.IsDeleted);
    }

    private IQueryable<RolePermissionSystemModuleOperation> GetRolePermissionSystemModuleOperationsQueryable()
    {
        return this._sqlServerContext.RolePermissionSystemModuleOperations
            .Include(x => x.Role)
            .Include(x => x.PermissionSystemModuleOperation)
                .ThenInclude(x => x.PermissionSystemModule)
                    .ThenInclude(x => x.PermissionSystem)
            .Include(x => x.PermissionSystemModuleOperation)
                .ThenInclude(x => x.PermissionSystemModule)
                    .ThenInclude(x => x.PermissionModule)
            .Include(x => x.PermissionSystemModuleOperation)
                .ThenInclude(x => x.PermissionOperation)
            .Where(x => !x.IsDeleted);
    }

    private static PermissionOperation CreatePermissionOperationData(string name)
    {
        PermissionOperation permission = new()
        {
            CreationTime = DateTime.UtcNow,
            Name = name,
            IsDeleted = false,
            Guid = Guid.NewGuid().ToString()
        };

        return permission;
    }

    private static PermissionSystem CreatePermissionSystemData(string name, bool enabled)
    {
        PermissionSystem permission = new()
        {
            CreationTime = DateTime.UtcNow,
            Name = name,
            Enabled = enabled,
            IsDeleted = false,
            Guid = Guid.NewGuid().ToString()
        };

        return permission;
    }

    private static PermissionModule CreatePermissionModuleData(string name, bool enabled)
    {
        PermissionModule permission = new()
        {
            CreationTime = DateTime.UtcNow,
            Name = name,
            Enabled = enabled,
            IsDeleted = false,
            Guid = Guid.NewGuid().ToString()
        };

        return permission;
    }

    private static PermissionSystemModule CreatePermissionSystemModuleData(
        PermissionSystem permissionSystem,
        PermissionModule permissionModule,
        bool enabled
    )
    {
        PermissionSystemModule permission = new()
        {
            CreationTime = DateTime.UtcNow,
            PermissionSystemId = permissionSystem.Id,
            PermissionSystem = permissionSystem,
            PermissionModuleId = permissionModule.Id,
            PermissionModule = permissionModule,
            Enabled = enabled,
            IsDeleted = false,
            Guid = Guid.NewGuid().ToString()
        };

        return permission;
    }

    private static PermissionSystemModuleOperation CreatePermissionSystemModuleOperationData(
        PermissionSystemModule permissionSystemModule,
        PermissionOperation permissionOperation,
        bool enabled
    )
    {
        PermissionSystemModuleOperation permission = new()
        {
            CreationTime = DateTime.UtcNow,
            PermissionOperationId = permissionOperation.Id,
            PermissionOperation = permissionOperation,
            PermissionSystemModuleId = permissionSystemModule.Id,
            PermissionSystemModule = permissionSystemModule,
            Enabled = enabled,
            IsDeleted = false,
            Guid = Guid.NewGuid().ToString()
        };

        return permission;
    }

    private static RolePermissionSystemModuleOperation CreateRolePermissionSystemModuleOperationData(
        Role role,
        PermissionSystemModuleOperation permissionModuleOperation,
        bool active
    )
    {
        RolePermissionSystemModuleOperation permission = new()
        {
            CreationTime = DateTime.UtcNow,
            PermissionSystemModuleOperationId = permissionModuleOperation.Id,
            PermissionSystemModuleOperation = permissionModuleOperation,
            RoleId = role.Id,
            Role = role,
            Active = active,
            IsDeleted = false,
            Guid = Guid.NewGuid().ToString()
        };

        return permission;
    }

    #region "PermissionSystem"

    public async Task<PermissionSystem?> GetPermissionSystemByGuidAsync(string guid)
    {
        return await this.GetPermissionSystemsQueryable().Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }

    public async Task<PermissionSystem?> GetPermissionSystemByNameAsync(string name)
    {
        return await this.GetPermissionSystemsQueryable().Where(x => x.Name == name).FirstOrDefaultAsync();
    }

    public async Task<PermissionSystem?> CreatePermissionSystemAsync(string name, bool enabled)
    {
        PermissionSystem? permission;

        using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            var tempPermission = CreatePermissionSystemData(name, enabled);
            await _sqlServerContext.PermissionSystems.AddAsync(tempPermission);
            await _sqlServerContext.SaveChangesAsync();
            await transaction.CommitAsync();
            permission = tempPermission;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            Logger.Error(exception, $"[PermissionService][CreatePermissionSystemAsync]");
            throw new CreateException($"An error occurred while creating the permission for transaction ID {transaction.TransactionId}.", exception);
        }
        return permission;
    }

    public async Task<bool?> HandleEnabledPermissionSystemAsync(PermissionSystem permission, bool enabled)
    {
        bool? updated = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.Enabled = enabled;
            permission.UpdateTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            updated = true;
        }

        return updated;
    }

    public async Task<bool?> DeletePermissionSystemAsync(PermissionSystem permission)
    {
        bool? deleted = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.IsDeleted = true;
            permission.DeletionTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            deleted = true;
        }

        return deleted;
    }

    #endregion


    #region "PermissionModule"


    public async Task<PermissionModule?> GetPermissionModuleByGuidAsync(string guid)
    {
        return await this.GetPermissionModulesQueryable().Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }

    public async Task<PermissionModule?> GetPermissionModuleByNameAsync(string name)
    {
        return await this.GetPermissionModulesQueryable().Where(x => x.Name == name).FirstOrDefaultAsync();
    }

    public async Task<PermissionModule?> CreatePermissionModuleAsync(string name, bool enabled)
    {
        PermissionModule? permission;

        using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            var tempPermission = CreatePermissionModuleData(name, enabled);
            await _sqlServerContext.PermissionModules.AddAsync(tempPermission);
            await _sqlServerContext.SaveChangesAsync();
            await transaction.CommitAsync();
            permission = tempPermission;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            Logger.Error(exception, $"[PermissionService][CreatePermissionModuleAsync]");
            throw new CreateException($"An error occurred while creating the permission for transaction ID {transaction.TransactionId}.", exception);
        }
        return permission;
    }

    public async Task<bool?> HandleEnabledPermissionModuleAsync(PermissionModule permission, bool enabled)
    {
        bool? updated = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.Enabled = enabled;
            permission.UpdateTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            updated = true;
        }

        return updated;
    }

    public async Task<bool?> DeletePermissionModuleAsync(PermissionModule permission)
    {
        bool? deleted = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.IsDeleted = true;
            permission.DeletionTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            deleted = true;
        }

        return deleted;
    }

    #endregion


    #region "PermissionOperation"

    public async Task<PermissionOperation?> GetPermissionOperationByGuidAsync(string guid)
    {
        return await this.GetPermissionOperationsQueryable().Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }

    public async Task<PermissionOperation?> GetPermissionOperationByNameAsync(string name)
    {
        return await this.GetPermissionOperationsQueryable().Where(x => x.Name == name).FirstOrDefaultAsync();
    }

    public async Task<PermissionOperation?> CreatePermissionOperationAsync(string name)
    {
        PermissionOperation? permission;

        using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            var tempPermission = CreatePermissionOperationData(name);
            await _sqlServerContext.PermissionOperations.AddAsync(tempPermission);
            await _sqlServerContext.SaveChangesAsync();
            await transaction.CommitAsync();
            permission = tempPermission;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            Logger.Error(exception, $"[PermissionService][CreatePermissionOperationAsync]");
            throw new CreateException($"An error occurred while creating the permission for transaction ID {transaction.TransactionId}.", exception);
        }
        return permission;
    }

    public async Task<bool?> DeletePermissionOperationAsync(PermissionOperation permission)
    {
        bool? deleted = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.IsDeleted = true;
            permission.DeletionTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            deleted = true;
        }

        return deleted;
    }

    #endregion


    #region "PermissionSystemModule"

    public async Task<PermissionSystemModule?> GetPermissionSystemModuleByGuidAsync(string guid)
    {
        return await this.GetPermissionSystemModulesQueryable().Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }

    public async Task<PermissionSystemModule?> CreatePermissionSystemModuleAsync(
        PermissionSystem permissionSystem,
        PermissionModule permissionModule,
        bool enabled
    )
    {
        PermissionSystemModule? permission;

        using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            var tempPermission = CreatePermissionSystemModuleData(permissionSystem, permissionModule, enabled);
            await _sqlServerContext.PermissionSystemModules.AddAsync(tempPermission);
            await _sqlServerContext.SaveChangesAsync();
            await transaction.CommitAsync();
            permission = tempPermission;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            Logger.Error(exception, $"[PermissionService][CreatePermissionSystemModuleAsync]");
            throw new CreateException($"An error occurred while creating the permission for transaction ID {transaction.TransactionId}.", exception);
        }
        return permission;
    }

    public async Task<bool?> HandleEnabledPermissionSystemModuleAsync(PermissionSystemModule permission, bool enabled)
    {
        bool? updated = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.Enabled = enabled;
            permission.UpdateTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            updated = true;
        }

        return updated;
    }

    public async Task<bool?> DeletePermissionSystemModuleAsync(PermissionSystemModule permission)
    {
        bool? deleted = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.IsDeleted = true;
            permission.DeletionTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            deleted = true;
        }

        return deleted;
    }

    #endregion


    #region "PermissionSystemModuleOperation"

    public async Task<PermissionSystemModuleOperation?> GetPermissionSystemModuleOperationByGuidAsync(string guid)
    {
        return await this.GetPermissionSystemModuleOperationsQueryable().Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }

    public async Task<PermissionSystemModuleOperation?> CreatePermissionSystemModuleOperationAsync(
        PermissionSystemModule permissionSystemModule,
        PermissionOperation permissionOperation,
        bool enabled
    )
    {
        PermissionSystemModuleOperation? permission;

        using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            var tempPermission = CreatePermissionSystemModuleOperationData(permissionSystemModule, permissionOperation, enabled);
            await _sqlServerContext.PermissionSystemModuleOperations.AddAsync(tempPermission);
            await _sqlServerContext.SaveChangesAsync();
            await transaction.CommitAsync();
            permission = tempPermission;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            Logger.Error(exception, $"[PermissionService][CreatePermissionSystemModuleOperationAsync]");
            throw new CreateException($"An error occurred while creating the permission for transaction ID {transaction.TransactionId}.", exception);
        }
        return permission;
    }

    public async Task<bool?> HandleEnabledPermissionSystemModuleOperationAsync(PermissionSystemModuleOperation permission, bool enabled)
    {
        bool? updated = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.Enabled = enabled;
            permission.UpdateTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            updated = true;
        }

        return updated;
    }

    public async Task<bool?> DeletePermissionSystemModuleOperationAsync(PermissionSystemModuleOperation permission)
    {
        bool? deleted = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.IsDeleted = true;
            permission.DeletionTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            deleted = true;
        }

        return deleted;
    }

    #endregion


    #region "RolePermissionSystemModuleOperation"

    public async Task<RolePermissionSystemModuleOperation?> GetRolePermissionSystemModuleOperationByGuidAsync(string guid)
    {
        return await this.GetRolePermissionSystemModuleOperationsQueryable().Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }

    public async Task<RolePermissionSystemModuleOperation?> CreateRolePermissionSystemModuleOperationAsync(
        Role role,
        PermissionSystemModuleOperation permissionSystemModuleOperation,
        bool enabled
    )
    {
        RolePermissionSystemModuleOperation? permission;

        using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            var tempPermission = CreateRolePermissionSystemModuleOperationData(role, permissionSystemModuleOperation, enabled);
            await _sqlServerContext.RolePermissionSystemModuleOperations.AddAsync(tempPermission);
            await _sqlServerContext.SaveChangesAsync();
            await transaction.CommitAsync();
            permission = tempPermission;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            Logger.Error(exception, $"[PermissionService][RolePermissionSystemModuleOperation]");
            throw new CreateException($"An error occurred while creating the permission for transaction ID {transaction.TransactionId}.", exception);
        }
        return permission;
    }

    public async Task<bool?> HandleEnabledRolePermissionSystemModuleOperationAsync(RolePermissionSystemModuleOperation permission, bool active)
    {
        bool? updated = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.Active = active;
            permission.UpdateTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            updated = true;
        }

        return updated;
    }

    public async Task<bool?> DeleteRolePermissionSystemModuleOperationAsync(RolePermissionSystemModuleOperation permission)
    {
        bool? deleted = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            permission.IsDeleted = true;
            permission.DeletionTime = DateTime.UtcNow;
            _sqlServerContext.Update(permission);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            deleted = true;
        }

        return deleted;
    }

    #endregion



    #region "CreatePermissionOnStartup"

    private static List<string?>? GetSystemNamesFromFile(PermissionsFile permissionsFile)
    {
        return permissionsFile?.PermissionInfos?.Where(x => x.System != null).Select(x => x.System)?.ToList();
    }

    private static List<string?>? GetModulesNamesFromFile(PermissionsFile permissionsFile)
    {
        return permissionsFile?.PermissionInfos?
            .Where(x => x.RolePermissionModuleOperations != null)
            .SelectMany(x => x.RolePermissionModuleOperations!)
            .Select(y => y.Module)
            .Distinct()
            .ToList();
    }

    private static List<string?>? GetModulesNamesFromPermissionInfo(PermissionInfo permissionInfo)
    {
        return permissionInfo.RolePermissionModuleOperations?
            .Select(y => y.Module)
            .Distinct()
            .ToList();
    }

    private (List<PermissionSystem>, List<string>) HandlePermissionSystemOnStartup(PermissionsFile permissionsFile)
    {
        List<string> newPermissions = [];
        List<string> systemNames = [];
        List<PermissionSystem> permissionSystemList = [];

        List<string?>? systems = GetSystemNamesFromFile(permissionsFile);
        if (systems != null && systems.Count > 0)
        {
            foreach (var system in systems)
            {
                if (!String.IsNullOrEmpty(system))
                {
                    systemNames.Add(system);
                }
            }
        }

        foreach (var system in systemNames)
        {
            PermissionSystem? permissionSystem = this.GetPermissionSystemByNameAsync(system).Result;
            if (permissionSystem == null)
            {
                permissionSystem = this.CreatePermissionSystemAsync(system, true).Result;
                newPermissions.Add($"Added new PermissionSystem => {permissionSystem?.Name}");
            }
            if (permissionSystem != null)
            {
                permissionSystemList.Add(permissionSystem);
            }
        }

        return (permissionSystemList, newPermissions);
    }

    private (List<PermissionModule>, List<string>) HandlePermissionModuleOnStartup(PermissionsFile permissionsFile)
    {
        List<string> newPermissions = [];
        List<string> moduleNames = [];
        List<PermissionModule> permissionModuleList = [];

        List<string?>? modules = GetModulesNamesFromFile(permissionsFile);

        if (modules != null && modules.Count > 0)
        {
            foreach (var module in modules)
            {
                if (!String.IsNullOrEmpty(module))
                {
                    moduleNames.Add(module);
                }
            }
        }

        foreach (var module in moduleNames)
        {
            PermissionModule? permissionModule = this.GetPermissionModuleByNameAsync(module).Result;
            if (permissionModule == null)
            {
                permissionModule = this.CreatePermissionModuleAsync(module, true).Result;
                newPermissions.Add($"Added new PermissionModule => {permissionModule?.Name}");
            }
            if (permissionModule != null)
            {
                permissionModuleList.Add(permissionModule);
            }
        }

        return (permissionModuleList, newPermissions);
    }

    private (List<PermissionOperation>, List<string>) HandlePermissionOperationOnStartup(PermissionsFile permissionsFile)
    {
        List<string> newPermissions = [];
        List<string> operationNames = [];
        List<PermissionOperation> permissionOperationList = [];

        List<string?>? operations = permissionsFile.PermissionInfos?
            .Where(x => x.RolePermissionModuleOperations != null)
            .SelectMany(x => x.RolePermissionModuleOperations!)
            .Where(x => x.Operations != null)
            .SelectMany(y => y.Operations!)
            .Select(z => z.Operation)
            .Distinct()
            .ToList();

        if (operations != null && operations.Count > 0)
        {
            foreach (var operation in operations)
            {
                if (!String.IsNullOrEmpty(operation))
                {
                    operationNames.Add(operation);
                }
            }
        }

        foreach (var operation in operationNames)
        {
            PermissionOperation? permissionOperation = this.GetPermissionOperationByNameAsync(operation).Result;
            if (permissionOperation == null)
            {
                permissionOperation = this.CreatePermissionOperationAsync(operation).Result;
                newPermissions.Add($"Added new PermissionOperation => {permissionOperation?.Name}");
            }
            if (permissionOperation != null)
            {
                permissionOperationList.Add(permissionOperation);
            }
        }

        return (permissionOperationList, newPermissions);
    }

    private async Task<List<Role>> HandleRolesOnStartup(PermissionsFile permissionsFile)
    {
        List<string> roleNames = [];
        List<Role> rolesList = [];

        List<string>? roles = permissionsFile.PermissionInfos?
            .Where(x => x.RolePermissionModuleOperations != null)
            .SelectMany(x => x.RolePermissionModuleOperations!)
            .Where(x => x.Operations != null)
            .SelectMany(y => y.Operations!)
            .Where(z => z.Roles != null)
            .SelectMany(z => z.Roles!)
            .Where(z => z != null)
            .Distinct()
            .ToList();

        if (roles != null && roles.Count > 0)
        {
            foreach (var role in roles)
            {
                if (!String.IsNullOrEmpty(role))
                {
                    roleNames.Add(role);
                }
            }
        }

        foreach (var roleName in roleNames)
        {
            Role? role = await this.GetRoleByNameQueryable(roleName).FirstOrDefaultAsync();
            if (role == null)
            {
                Role tempRole = new()
                {
                    CreationTime = DateTime.UtcNow,
                    IsDeleted = false,
                    Guid = Guid.NewGuid().ToString(),
                    Name = roleName,
                    IsNotEditable = false
                };
                using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();
                try
                {
                    await _sqlServerContext.Roles.AddAsync(tempRole);
                    await _sqlServerContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    role = tempRole;
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                    Logger.Error(exception, $"[RoleService][CreateRoleAsync]");
                    throw new CreateException($"An error occurred while saving the role for transaction ID {transaction.TransactionId}.", exception);
                }

                Logger.Info($"Added new Role => {role?.Name}");
            }
            if (role != null)
            {
                rolesList.Add(role);
            }
        }

        return rolesList;
    }

    private (List<PermissionSystemModule>, List<string>) HandlePermissionSystemModulesOnStartup(PermissionSystem permissionSystem, List<PermissionModule> permissionModules)
    {
        List<string> newPermissions = [];
        List<PermissionSystemModule> permissionSystemModuleList = [];

        foreach (var permissionModule in permissionModules)
        {
            PermissionSystemModule? permissionSystemModule = this.GetPermissionSystemModulesQueryable()?
                .Where(x =>
                    x.PermissionSystemId == permissionSystem!.Id &&
                    x.PermissionModuleId == permissionModule.Id
                )?.FirstOrDefault();
            if (permissionSystemModule == null)
            {
                permissionSystemModule = this.CreatePermissionSystemModuleAsync(permissionSystem!, permissionModule, true).Result;
                newPermissions.Add($"Added new PermissionSystemModule => {permissionSystem?.Name}.{permissionModule?.Name}");
            }
            if (permissionSystemModule != null)
            {
                permissionSystemModuleList.Add(permissionSystemModule);
            }
        }

        return (permissionSystemModuleList, newPermissions);
    }

    private (List<PermissionSystemModule>, List<string>) HandlePermissionSystemModuleOnStartup
    (
        PermissionsFile permissionsFile,
        List<PermissionSystem> permissionSystems,
        List<PermissionModule> allPermissionModules,
        PermissionInfo permissionInfo
    )
    {
        List<string> newPermissions = [];
        List<PermissionSystemModule> permissionSystemModuleList = [];

        PermissionSystem? permissionSystem = permissionSystems.FirstOrDefault(x => x.Name == permissionInfo.System);
        if (permissionSystem != null)
        {
            List<string?>? modules = GetModulesNamesFromFile(permissionsFile);
            if (modules != null && modules.Count > 0)
            {
                List<PermissionModule> permissionModules = allPermissionModules.Where(x => modules.Contains(x.Name)).ToList();
                if (permissionModules != null && permissionModules.Count > 0)
                {
                    (permissionSystemModuleList, newPermissions) = this.HandlePermissionSystemModulesOnStartup(permissionSystem, permissionModules);
                }
            }
        }

        return (permissionSystemModuleList, newPermissions);
    }
    private (List<PermissionSystemModule>, List<string>) HandlePermissionSystemModuleOnStartup(
        PermissionsFile permissionsFile,
        List<PermissionSystem> permissionSystems,
        List<PermissionModule> allPermissionModules
    )
    {
        List<string> newPermissions = [];
        List<PermissionSystemModule> permissionSystemModuleList = [];

        if (permissionsFile?.PermissionInfos != null)
        {
            foreach (var permissionInfo in permissionsFile!.PermissionInfos!)
            {
                if (!String.IsNullOrEmpty(permissionInfo.System))
                {
                    var modulesNames = GetModulesNamesFromPermissionInfo(permissionInfo);
                    if (modulesNames != null && modulesNames.Count > 0)
                    {
                        List<PermissionModule> permissionModules = allPermissionModules.Where(x => modulesNames.Contains(x.Name))?.ToList() ?? [];
                        (permissionSystemModuleList, newPermissions) = this.HandlePermissionSystemModuleOnStartup(permissionsFile, permissionSystems, permissionModules, permissionInfo);
                    }
                }
            }
        }

        return (permissionSystemModuleList, newPermissions);
    }

    private (List<PermissionSystemModuleOperation>, List<string>) HandlePermissionSystemModuleOperationOnStartup
    (
        PermissionSystemModule permissionSystemModule,
        List<PermissionOperation> permissionOperations
    )
    {
        List<string> newPermissions = [];
        List<PermissionSystemModuleOperation> permissionSystemModuleOperationList = [];

        foreach (var permissionOperation in permissionOperations)
        {
            PermissionSystemModuleOperation? permissionSystemModuleOperation = this.GetPermissionSystemModuleOperationsQueryable()?
                .FirstOrDefault(x =>
                    x.PermissionSystemModuleId == permissionSystemModule!.Id &&
                    x.PermissionOperationId == permissionOperation.Id
                );
            if (permissionSystemModuleOperation == null)
            {
                permissionSystemModuleOperation = this.CreatePermissionSystemModuleOperationAsync(permissionSystemModule!, permissionOperation, true).Result;
                newPermissions.Add($"Added new PermissionSystemModuleOperation => {permissionSystemModuleOperation?.PermissionSystemModule?.PermissionSystem?.Name}.{permissionSystemModuleOperation?.PermissionSystemModule?.PermissionModule?.Name}.{permissionSystemModuleOperation?.PermissionOperation?.Name}");
            }
            if (permissionSystemModuleOperation != null)
            {
                permissionSystemModuleOperationList.Add(permissionSystemModuleOperation!);
            }
        }

        return (permissionSystemModuleOperationList, newPermissions);
    }

    private (List<PermissionSystemModuleOperation>, List<string>) HandlePermissionSystemModuleOperationOnStartup
    (
        List<PermissionSystemModule> permissionSystemModulesList,
        List<PermissionOperation> allPermissionOperations,
        PermissionInfo permissionInfo
    )
    {
        List<string> newPermissions = [];
        List<string> tmpPermissions = [];
        List<PermissionSystemModuleOperation> permissionSystemModuleOperationList = [];
        List<PermissionSystemModuleOperation> tmpPermissionSystemModuleOperationList = [];

        if (permissionInfo != null && permissionInfo.RolePermissionModuleOperations != null)
        {
            foreach (var rolePermissionModuleOperation in permissionInfo.RolePermissionModuleOperations)
            {
                PermissionSystemModule? permissionSystemModule = permissionSystemModulesList.FirstOrDefault(x => x.PermissionModule.Name == rolePermissionModuleOperation.Module);
                if (permissionSystemModule != null)
                {
                    var operationsNames = rolePermissionModuleOperation.Operations?.Select(x => x.Operation).ToList();
                    if (operationsNames != null && operationsNames.Count > 0)
                    {
                        List<PermissionOperation> permissionOperations = allPermissionOperations.Where(x => operationsNames.Contains(x.Name)).ToList();
                        (tmpPermissionSystemModuleOperationList, tmpPermissions) = this.HandlePermissionSystemModuleOperationOnStartup(permissionSystemModule, permissionOperations);
                        newPermissions.AddRange(tmpPermissions);
                        permissionSystemModuleOperationList.AddRange(tmpPermissionSystemModuleOperationList);
                    }
                }
            }
        }

        return (permissionSystemModuleOperationList, newPermissions);
    }

    private (List<PermissionSystemModuleOperation>, List<string>) HandlePermissionSystemModuleOperationOnStartup
    (
        PermissionsFile permissionsFile,
        List<PermissionSystemModule> permissionSystemModules,
        List<PermissionOperation> allPermissionOperation
    )
    {
        List<string> newPermissions = [];
        List<string> tmpPermissions = [];
        List<PermissionSystemModuleOperation> permissionSystemModuleOperationList = [];
        List<PermissionSystemModuleOperation> tmpPermissionSystemModuleOperationList = [];

        if (permissionsFile.PermissionInfos != null)
        {
            foreach (var permissionInfo in permissionsFile.PermissionInfos!)
            {
                if (!String.IsNullOrEmpty(permissionInfo.System))
                {
                    // Get all PermissionSystemModules by System.Name
                    List<PermissionSystemModule> permissionSystemModulesList = permissionSystemModules
                        .Where(x => x.PermissionSystem.Name == permissionInfo.System).ToList();

                    if (permissionSystemModulesList != null && permissionSystemModulesList.Count > 0)
                    {
                        (tmpPermissionSystemModuleOperationList, tmpPermissions) = this.HandlePermissionSystemModuleOperationOnStartup
                        (
                            permissionSystemModulesList,
                            allPermissionOperation,
                            permissionInfo
                        );
                        newPermissions.AddRange(tmpPermissions);
                        permissionSystemModuleOperationList.AddRange(tmpPermissionSystemModuleOperationList);
                    }
                }
            }
        }

        return (permissionSystemModuleOperationList, newPermissions);
    }

    private (List<RolePermissionSystemModuleOperation>, List<string>) HandleRolePermissionSystemModuleOperationOnStartup
    (
        List<PermissionSystemModuleOperation> allPermissionSystemModuleOperationsBySystem,
        List<Role> allRoles,
        PermissionInfo permissionInfo
    )
    {
        List<string> newPermissions = [];
        List<string> tmpPermissions = [];
        List<RolePermissionSystemModuleOperation> rolePermissionSystemModuleOperationList = [];
        List<RolePermissionSystemModuleOperation> tmpRolePermissionSystemModuleOperationList = [];

        if (permissionInfo != null && permissionInfo.RolePermissionModuleOperations != null)
        {
            foreach (var rolePermissionModuleOperation in permissionInfo.RolePermissionModuleOperations)
            {
                List<PermissionSystemModuleOperation>? allPermissionSystemModuleOperationsBySystemModule = allPermissionSystemModuleOperationsBySystem
                    .Where(x => x.PermissionSystemModule.PermissionModule.Name == rolePermissionModuleOperation.Module)
                    .ToList();
                if (allPermissionSystemModuleOperationsBySystemModule != null && allPermissionSystemModuleOperationsBySystemModule.Count > 0)
                {
                    var operationsNames = rolePermissionModuleOperation.Operations?.Select(x => x.Operation).ToList();
                    if (operationsNames != null && operationsNames.Count > 0)
                    {
                        List<PermissionSystemModuleOperation> permissionSystemModuleOperations = allPermissionSystemModuleOperationsBySystemModule
                            .Where(x => operationsNames.Contains(x.PermissionOperation.Name)).ToList();
                        (tmpRolePermissionSystemModuleOperationList, tmpPermissions) = this.HandleRolePermissionSystemModuleOperationOnStartup(
                            permissionSystemModuleOperations,
                            allRoles,
                            rolePermissionModuleOperation
                        );
                        newPermissions.AddRange(tmpPermissions);
                        rolePermissionSystemModuleOperationList.AddRange(tmpRolePermissionSystemModuleOperationList);
                    }
                }
            }
        }

        return (rolePermissionSystemModuleOperationList, newPermissions);
    }

    private (List<RolePermissionSystemModuleOperation>, List<string>) HandleRolePermissionSystemModuleOperationOnStartup
    (
        List<PermissionSystemModuleOperation> permissionSystemModuleOperations,
        List<Role> allRoles,
        RolePermissionModuleOperation rolePermissionModuleOperation
    )
    {
        List<string> newPermissions = [];
        List<string> tmpPermissions = [];
        List<RolePermissionSystemModuleOperation> rolePermissionSystemModuleOperationList = [];
        List<RolePermissionSystemModuleOperation> tmpRolePermissionSystemModuleOperationList = [];

        if (permissionSystemModuleOperations != null && permissionSystemModuleOperations.Count > 0 &&
            allRoles != null && allRoles.Count > 0 && rolePermissionModuleOperation?.Operations != null &&
            rolePermissionModuleOperation.Operations.Count > 0
        )
        {
            foreach (var operationInfo in rolePermissionModuleOperation.Operations)
            {
                PermissionSystemModuleOperation? permissionSystemModuleOperation = permissionSystemModuleOperations
                    .FirstOrDefault(x => x.PermissionOperation.Name == operationInfo.Operation);
                if (permissionSystemModuleOperation != null && operationInfo.Roles != null && operationInfo.Roles.Count > 0)
                {
                    var roles = allRoles.Where(x => operationInfo.Roles.Contains(x.Name))?.ToList() ?? [];
                    if (roles != null && roles.Count > 0)
                    {
                        foreach (var roleName in operationInfo.Roles)
                        {
                            (tmpRolePermissionSystemModuleOperationList, tmpPermissions) = this.HandleRolePermissionSystemModuleOperationOnStartup
                            (
                                roles, roleName, permissionSystemModuleOperation
                            );
                            newPermissions.AddRange(tmpPermissions);
                            rolePermissionSystemModuleOperationList.AddRange(tmpRolePermissionSystemModuleOperationList);
                        }

                    }
                }
            }
        }

        return (rolePermissionSystemModuleOperationList, newPermissions);
    }

    private (List<RolePermissionSystemModuleOperation>, List<string>) HandleRolePermissionSystemModuleOperationOnStartup
    (
        List<Role> roles, string roleName, PermissionSystemModuleOperation permissionSystemModuleOperation
    )
    {
        List<string> newPermissions = [];
        List<RolePermissionSystemModuleOperation> rolePermissionSystemModuleOperationList = [];

        Role? role = roles.FirstOrDefault(x => x.Name == roleName);
        if (role != null)
        {
            RolePermissionSystemModuleOperation? rolePermissionSystemModuleOperation = this._sqlServerContext.RolePermissionSystemModuleOperations?
                .FirstOrDefault(x => x.RoleId == role.Id && x.PermissionSystemModuleOperationId == permissionSystemModuleOperation!.Id);
            if (rolePermissionSystemModuleOperation == null)
            {
                rolePermissionSystemModuleOperation = this.CreateRolePermissionSystemModuleOperationAsync(role, permissionSystemModuleOperation!, true).Result;
                if (rolePermissionSystemModuleOperation != null)
                {
                    newPermissions.Add($"Added new RolePermissionSystemModuleOperation => " +
                        $"{permissionSystemModuleOperation?.PermissionSystemModule?.PermissionSystem?.Name}." +
                        $"{permissionSystemModuleOperation?.PermissionSystemModule?.PermissionModule?.Name}." +
                        $"{permissionSystemModuleOperation?.PermissionOperation?.Name} for role {role.Name}");
                }
                if (rolePermissionSystemModuleOperation != null)
                {
                    rolePermissionSystemModuleOperationList.Add(rolePermissionSystemModuleOperation!);
                }
            }
        }

        return (rolePermissionSystemModuleOperationList, newPermissions);
    }

    private (List<RolePermissionSystemModuleOperation>, List<string>) HandleRolePermissionSystemModuleOperationOnStartup
    (
        PermissionsFile permissionsFile,
        List<PermissionSystemModuleOperation> allPermissionSystemModuleOperations,
        List<Role> allRoles
    )
    {
        List<string> newPermissions = [];
        List<string> tmpPermissions = [];
        List<RolePermissionSystemModuleOperation> rolePermissionSystemModuleOperationList = [];
        List<RolePermissionSystemModuleOperation> tmpRolePermissionSystemModuleOperationList = [];

        if (permissionsFile.PermissionInfos != null)
        {
            foreach (var permissionInfo in permissionsFile.PermissionInfos!)
            {
                if (!String.IsNullOrEmpty(permissionInfo.System))
                {
                    // Get all PermissionSystemModuleOperations by System.Name
                    List<PermissionSystemModuleOperation> allPermissionSystemModuleOperationsBySystem = allPermissionSystemModuleOperations
                        .Where(x => x.PermissionSystemModule.PermissionSystem.Name == permissionInfo.System)?.ToList() ?? [];

                    if (allPermissionSystemModuleOperationsBySystem != null && allPermissionSystemModuleOperationsBySystem.Count > 0)
                    {
                        (tmpRolePermissionSystemModuleOperationList, tmpPermissions) = this.HandleRolePermissionSystemModuleOperationOnStartup
                        (
                            allPermissionSystemModuleOperationsBySystem,
                            allRoles,
                            permissionInfo
                        );
                        newPermissions.AddRange(tmpPermissions);
                        rolePermissionSystemModuleOperationList.AddRange(tmpRolePermissionSystemModuleOperationList);
                    }
                }
            }
        }

        return (rolePermissionSystemModuleOperationList, newPermissions);

    }

    public List<string> CreatePermissionsOnStartupAsync()
    {
        try
        {
            List<string> tmpPermissions = [];
            List<string> newPermissions = [];
            PermissionsFile? permissionsFile = FileUtils.ConvertFileToObject<PermissionsFile>(System.AppDomain.CurrentDomain.BaseDirectory + this._appSettings?.PermissionsSettings?.FilePath);

            List<PermissionSystem> permissionSystemList = [];
            List<PermissionModule> permissionModuleList = [];
            List<PermissionOperation> permissionOperationList = [];
            List<PermissionSystemModule> permissionSystemModuleList = [];
            List<PermissionSystemModuleOperation> permissionSystemModuleOperationList = [];
            List<RolePermissionSystemModuleOperation> rolePermissionSystemModuleOperationList = [];

            if (permissionsFile != null && permissionsFile.PermissionInfos != null && permissionsFile.PermissionInfos.Count > 0)
            {
                (permissionSystemList, tmpPermissions) = this.HandlePermissionSystemOnStartup(permissionsFile);
                newPermissions.AddRange(tmpPermissions);

                (permissionModuleList, tmpPermissions) = this.HandlePermissionModuleOnStartup(permissionsFile);
                newPermissions.AddRange(tmpPermissions);

                (permissionOperationList, tmpPermissions) = this.HandlePermissionOperationOnStartup(permissionsFile);
                newPermissions.AddRange(tmpPermissions);

                (permissionSystemModuleList, tmpPermissions) = this.HandlePermissionSystemModuleOnStartup(permissionsFile, permissionSystemList, permissionModuleList);
                newPermissions.AddRange(tmpPermissions);

                (permissionSystemModuleOperationList, tmpPermissions) = this.HandlePermissionSystemModuleOperationOnStartup(permissionsFile, permissionSystemModuleList, permissionOperationList);
                newPermissions.AddRange(tmpPermissions);

                List<Role> roles = this.HandleRolesOnStartup(permissionsFile).Result;

                (rolePermissionSystemModuleOperationList, tmpPermissions) = this.HandleRolePermissionSystemModuleOperationOnStartup(
                    permissionsFile,
                    permissionSystemModuleOperationList,
                    roles
                );
                newPermissions.AddRange(tmpPermissions);
            }

            return newPermissions;
        }
        catch (Exception exception)
        {
            Logger.Error(exception, $"[PermissionService][CreatePermissionsOnStartupAsync]");
            throw new CreateException($"An error occurred while adding permissions during startup", exception);
        }

    }



    #endregion



}