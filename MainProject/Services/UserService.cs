
using System.Collections;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace BasicDotnetTemplate.MainProject.Services;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByGuidAsync(string guid);
    Task<User?> GetUserByUsernameAndPassword(string username, string password);
}

public class UserService : BaseService, IUserService
{

    public UserService(
        IConfiguration configuration,
        SqlServerContext sqlServerContext
    ) : base(configuration, sqlServerContext)
    { }

    private IQueryable<User> GetUsers()
    {
        return this._sqlServerContext.Users.Where(x => !x.IsDeleted);
    }

    private IQueryable<User> GetUserByUsername(string username)
    {
        return this.GetUsers().Where(x =>
            x.Username.ToString() == username.ToString()
        );
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await this.GetUsers().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByGuidAsync(string guid)
    {
        return await this.GetUsers().Where(x => x.Guid == guid).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByUsernameAndPassword(string username, string password)
    {
        User? user = null;

        try
        {
            user = await this.GetUserByUsername(username).FirstOrDefaultAsync();
            if (user != null)
            {
                var encryptedPassword = user.PasswordHash;
                Console.WriteLine(encryptedPassword);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }

        return user;
    }

    // public async Task<User?> CreateUser(CreateUserRequestData data)
    // {

    // }


}

