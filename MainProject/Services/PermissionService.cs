
using System.Collections;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Models.Api.Common.Exceptions;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace BasicDotnetTemplate.MainProject.Services;

public interface IPermissionService
{
    Task<PermissionSystem?> GetPermissionSystemByGuidAsync(string guid);
    Task<PermissionSystem?> GetPermissionSystemByNameAsync(string name);
    Task<bool?> HandleEnabledPermissionSystem(PermissionSystem permission, bool enabled);
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
}

public class PermissionService : BaseService, IPermissionService
{
    private readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    public PermissionService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        SqlServerContext sqlServerContext
    ) : base(httpContextAccessor, configuration, sqlServerContext)
    { }

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

    public async Task<bool?> HandleEnabledPermissionSystem(PermissionSystem permission, bool enabled)
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

}

