
using System.Collections;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace BasicDotnetTemplate.MainProject.Services;

public interface IUserService
{
    User? GetUserById(int id);
    User? GetUserByGuid(string guid);
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

    public User? GetUserById(int id)
    {
        return this.GetUsers().Where(x => x.Id == id).FirstOrDefault();
    }

    public User? GetUserByGuid(string guid)
    {
        return this.GetUsers().Where(x => x.Guid == guid).FirstOrDefault();
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
}

