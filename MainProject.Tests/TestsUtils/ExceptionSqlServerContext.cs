using BasicDotnetTemplate.MainProject.Core.Database;
using Microsoft.EntityFrameworkCore;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Newtonsoft.Json;

namespace BasicDotnetTemplate.MainProject.Tests;

public class ExceptionSqlServerContext : SqlServerContext
{
    public bool ThrowExceptionOnSave { get; set; }

    public ExceptionSqlServerContext() : base(TestUtils.CreateInMemorySqlContextOptions())
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (ThrowExceptionOnSave)
        {
            throw new Exception("Database error");
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}