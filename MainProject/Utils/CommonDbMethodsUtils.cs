using System;
using System.Security.Cryptography;
using System.Text;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;

namespace BasicDotnetTemplate.MainProject.Utils;
public class CommonDbMethodsUtils
{
    private readonly SqlServerContext _sqlServerContext;

    public CommonDbMethodsUtils(SqlServerContext sqlServerContext)
    {
        _sqlServerContext = sqlServerContext;
    }


    public IQueryable<Role> GetRolesQueryable()
    {
        return this._sqlServerContext.Roles.Where(x => !x.IsDeleted);
    }

    public IQueryable<Role> GetRoleByNameQueryable(string name)
    {
        return this.GetRolesQueryable().Where(x =>
            x.Name.ToString() == name.ToString()
        );
    }


}

