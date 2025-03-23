
using System.Collections;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace BasicDotnetTemplate.MainProject.Services;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByGuidAsync(string guid);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameAndPassword(string email, string password);
    Task<bool> CheckIfEmailIsValid(string email, string? guid = "");
    Task<User?> CreateUserAsync(CreateUserRequestData data, Role role);
    Task<bool?> DeleteUserAsync(User user);
}

public class UserService : BaseService, IUserService
{
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
        User user = new()
        {
            CreationTime = DateTime.UtcNow,
            CreationUserId = this.GetCurrentUserId(),
            IsDeleted = false,
            Guid = Guid.NewGuid().ToString(),
            FirstName = data.FirstName,
            LastName = data.LastName,
            Email = data.Email,
            PasswordSalt = "",
            PasswordHash = "",
            Password = "",
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

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await this.GetUserByEmailQueryable(email).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByUsernameAndPassword(string email, string password)
    {
        User? user = await this.GetUserByEmailQueryable(email).FirstOrDefaultAsync();
        if (user != null)
        {
            var encryptedPassword = user.PasswordHash;
            Console.WriteLine(encryptedPassword);
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
        User? user = null;

        await using var transaction = await _sqlServerContext.Database.BeginTransactionAsync();

        try
        {
            var tempUser = this.CreateUserData(data, role);
            await _sqlServerContext.Users.AddAsync(tempUser);
            await _sqlServerContext.SaveChangesAsync();

            await transaction.CommitAsync();
            user = tempUser;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
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


}

