using Microsoft.EntityFrameworkCore;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;


namespace BasicDotnetTemplate.MainProject.Core.Database
{
    public class SqlServerContext : DbContext
    {

        public SqlServerContext(DbContextOptions<SqlServerContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email, "IX_Email");

            modelBuilder.Entity<User>()
                .HasIndex(x => new { x.IsDeleted, x.Guid }, "IX_IsDeleted_Guid")
                .HasFilter("[IsDeleted] = 0");


            modelBuilder.Entity<Role>()
                .HasIndex(x => new { x.IsDeleted, x.Guid }, "IX_IsDeleted_Guid")
                .HasFilter("[IsDeleted] = 0");
        }
    }
}



