using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;


namespace BasicDotnetTemplate.MainProject.Tests;

public static class ModelsInit
{
    public static DatabaseSqlServer.User CreateUser()
    {
        DatabaseSqlServer.User user = new DatabaseSqlServer.User()
        {
            Guid = Guid.NewGuid().ToString(),
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "Email",
            PasswordHash = "PasswordHash",
            PasswordSalt = "PasswordSalt",
            Password = "Password",
            Role = CreateRole(),
            IsTestUser = true
        };
        return user;
    }

    public static DatabaseSqlServer.Role CreateRole()
    {
        DatabaseSqlServer.Role role = new DatabaseSqlServer.Role()
        {
            Guid = Guid.NewGuid().ToString(),
            Name = "Name",
            IsNotEditable = false
        };
        return role;
    }
}