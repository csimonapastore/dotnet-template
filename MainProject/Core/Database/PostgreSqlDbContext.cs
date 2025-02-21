using Microsoft.EntityFrameworkCore;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;


namespace BasicDotnetTemplate.MainProject.Core.Database
{
    public class PostgreSqlDbContext : DbContext
    {

        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options)
        : base(options)
        {
        }
    }
}



