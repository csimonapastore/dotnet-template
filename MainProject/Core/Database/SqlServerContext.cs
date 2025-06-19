using Microsoft.EntityFrameworkCore;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;


namespace BasicDotnetTemplate.MainProject.Core.Database
{
    public class SqlServerContext : DbContext
    {
        private const string _isDeletedFalse = "[IsDeleted] = 0";
        private const string _isEnabled = "[Enabled] = 1";

        public SqlServerContext(DbContextOptions<SqlServerContext> options)
        : base(options)
        {
        }

        public DbSet<PermissionModule> PermissionModules { get; set; }
        public DbSet<PermissionOperation> PermissionOperations { get; set; }
        public DbSet<PermissionSystem> PermissionSystems { get; set; }
        public DbSet<PermissionSystemModule> PermissionSystemModules { get; set; }
        public DbSet<PermissionSystemModuleOperation> PermissionSystemModuleOperations { get; set; }
        public DbSet<RolePermissionSystemModuleOperation> RolePermissionSystemModuleOperations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region "INDEXES"
            // Indexes

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email, "IX_Email");

            modelBuilder.Entity<User>()
                .HasIndex(x => new { x.IsDeleted, x.Guid }, "IX_IsDeleted_Guid")
                .HasFilter(_isDeletedFalse);

            modelBuilder.Entity<Role>()
                .HasIndex(x => new { x.IsDeleted, x.Guid }, "IX_IsDeleted_Guid")
                .HasFilter(_isDeletedFalse);

            modelBuilder.Entity<PermissionSystem>()
                .HasIndex(x => new { x.IsDeleted }, "IX_IsDeleted")
                .HasFilter(_isDeletedFalse);

            modelBuilder.Entity<PermissionSystem>()
                .HasIndex(x => new { x.Enabled }, "IX_Enabled")
                .HasFilter(_isEnabled);

            modelBuilder.Entity<PermissionSystem>()
                .HasIndex(x => new { x.IsDeleted, x.Name, x.Enabled }, "IX_IsDeleted_Name_Enabled")
                .HasFilter(_isEnabled)
                .HasFilter(_isDeletedFalse);

            modelBuilder.Entity<PermissionModule>()
                .HasIndex(x => new { x.IsDeleted }, "IX_IsDeleted")
                .HasFilter(_isDeletedFalse);

            modelBuilder.Entity<PermissionModule>()
                .HasIndex(x => new { x.Enabled }, "IX_Enabled")
                .HasFilter(_isEnabled);

            modelBuilder.Entity<PermissionModule>()
                .HasIndex(x => new { x.IsDeleted, x.Name, x.Enabled }, "IX_IsDeleted_Name_Enabled")
                .HasFilter(_isEnabled)
                .HasFilter(_isDeletedFalse);

            modelBuilder.Entity<PermissionOperation>()
                .HasIndex(x => new { x.IsDeleted, x.Name }, "IX_IsDeleted_Name");

            modelBuilder.Entity<PermissionSystemModuleOperation>()
                .HasIndex(x => new { x.IsDeleted, x.Enabled, x.Guid }, "IX_IsDeleted_Enabled_Guid");

            #endregion

        }
    }
}



