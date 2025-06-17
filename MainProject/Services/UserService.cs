
using System.Collections;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Models.Api.Common.Exceptions;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using BasicDotnetTemplate.MainProject.Utils;
using Microsoft.EntityFrameworkCore;

namespace BasicDotnetTemplate.MainProject.Services;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByGuidAsync(string guid);
    Task<User?> GetUserByUsernameAndPassword(string email, string password);
    Task<bool> CheckIfEmailIsValid(string email, string? guid = "");
    Task<User?> CreateUserAsync(CreateUserRequestData data, Role role);
    Task<User?> UpdateUserAsync(UpdateUserRequestData data, User user);
    Task<bool?> DeleteUserAsync(User user);
    Task<User?> UpdateUserPasswordAsync(User user, string newPassword);
    Task<User?> UpdateUserRoleAsync(User user, Role newRole);
}

public class UserService : BaseService, IUserService
{
    private readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    public UserService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        SqlServerContext sqlServerContext
    ) : base(httpContextAccessor, configuration, sqlServerContext)
    { }

    private IQueryable<User> GetUsersQueryable()
    {
        return this._sqlServerContext.Users.Where(x => !x.IsDeleted);
    }

    private IQueryable<User> GetUserByEmailQueryable(string email)
    {
        return this.GetUsersQueryable().Where(x =>
            x.Email.ToString() == email.ToString()
        );
    }

    private User CreateUserData(CreateUserRequestData data, Role role)
    {
        var salt = _appSettings.EncryptionSettings?.Salt ?? String.Empty;
        var pepper = CryptUtils.GeneratePepper();
        var iterations = _appSettings.EncryptionSettings?.Iterations ?? 10;
        User user = new()
        {
            CreationTime = DateTime.UtcNow,
            CreationUserId = this.GetCurrentUserId(),
            IsDeleted = false,
            Guid = Guid.NewGuid().ToString(),
            FirstName = data.FirstName,
            LastName = data.LastName,
            Email = data.Email,
            PasswordSalt = salt,
            PasswordPepper = pepper,
            PasswordIterations = iterations,
            Password = CryptUtils.GeneratePassword(data.Password, salt, iterations, pepper),
            Role = role,
            IsTestUser = false
        };

        return user;
    }


    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await this.GetUsersQueryable().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByGuidAsync(string guid)
    {
        return await this.GetUsersQueryable().Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByUsernameAndPassword(string email, string password)
    {
        User? user = await this.GetUserByEmailQueryable(email).FirstOrDefaultAsync();
        if (user != null)
        {
            var valid = CryptUtils.VerifyPassword(user.Password, password, user.PasswordSalt, user.PasswordIterations, user.PasswordPepper);
            if (!valid)
                user = null;
        }

        return user;
    }

    public async Task<bool> CheckIfEmailIsValid(string email, string? guid = "")
    {
        var valid = false;

        User? user = await this.GetUserByEmailQueryable(email).FirstOrDefaultAsync();
        if (user != null)
        {
            if (!String.IsNullOrEmpty(guid))
            {
                valid = user.Guid == guid && user.Email == email;
            }
        }
        else
        {
            valid = true;
        }
        return valid;
    }

    public async Task<User?> CreateUserAsync(CreateUserRequestData data, Role role)
    {
        User? user;

        using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            var tempUser = CreateUserData(data, role);
            await _sqlServerContext.Users.AddAsync(tempUser);
            await _sqlServerContext.SaveChangesAsync();
            await transaction.CommitAsync();
            user = tempUser;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            Logger.Error(exception, $"[UserService][CreateUserAsync]");
            throw new CreateException($"An error occurred while creating the user for transaction ID {transaction.TransactionId}.", exception);
        }


        return user;
    }

    public async Task<User?> UpdateUserAsync(UpdateUserRequestData data, User user)
    {
        using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            user.FirstName = data.FirstName ?? user.FirstName;
            user.LastName = data.LastName ?? user.LastName;
            user.UpdateTime = DateTime.UtcNow;
            user.UpdateUserId = this.GetCurrentUserId();

            _sqlServerContext.Users.Update(user);
            await _sqlServerContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception exception)
        {
            Logger.Error(exception, $"[UserService][UpdateUserAsync] | {transaction.TransactionId}");
            await transaction.RollbackAsync();
            throw new UpdateException($"An error occurred while updating the user for transaction ID {transaction.TransactionId}.", exception);
        }

        return user;
    }

    public async Task<bool?> DeleteUserAsync(User user)
    {
        bool? deleted = false;

        using (var transaction = _sqlServerContext.Database.BeginTransactionAsync())
        {
            user.IsDeleted = true;
            user.DeletionTime = DateTime.UtcNow;
            _sqlServerContext.Update(user);
            await _sqlServerContext.SaveChangesAsync();
            await (await transaction).CommitAsync();
            deleted = true;
        }

        return deleted;
    }

    public async Task<User?> UpdateUserPasswordAsync(User user, string newPassword)
    {
        using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            var salt = _appSettings.EncryptionSettings?.Salt ?? String.Empty;
            var pepper = CryptUtils.GeneratePepper();
            var iterations = _appSettings.EncryptionSettings?.Iterations ?? 10;

            user.PasswordSalt = salt;
            user.PasswordPepper = pepper;
            user.PasswordIterations  = iterations;
            user.Password = CryptUtils.GeneratePassword(newPassword, salt, iterations, pepper);
            user.UpdateTime = DateTime.UtcNow;
            user.UpdateUserId = this.GetCurrentUserId();

            _sqlServerContext.Users.Update(user);
            await _sqlServerContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception exception)
        {
            Logger.Error(exception, $"[UserService][UpdateUserPasswordAsync] | {transaction.TransactionId}");
            await transaction.RollbackAsync();
            throw new UpdateException($"An error occurred while updating the user for transaction ID {transaction.TransactionId}.", exception);
        }

        return user;
    }

    public async Task<User?> UpdateUserRoleAsync(User user, Role newRole)
    {
        using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            user.Role = newRole;
            user.UpdateTime = DateTime.UtcNow;
            user.UpdateUserId = this.GetCurrentUserId();

            _sqlServerContext.Users.Update(user);
            await _sqlServerContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception exception)
        {
            Logger.Error(exception, $"[UserService][UpdateUserRoleAsync] | {transaction.TransactionId}");
            await transaction.RollbackAsync();
            throw new UpdateException($"An error occurred while updating the user for transaction ID {transaction.TransactionId}.", exception);
        }

        return user;
    }
}

