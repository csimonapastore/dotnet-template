
using System.Collections;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace BasicDotnetTemplate.MainProject.Services;

public interface IUserService
{
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
        return this._sqlServerContext.Users
            .Where(x =>
                !x.IsDeleted &&
                String.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase)
            );
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

