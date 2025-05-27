using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainProject.Migrations
{
    /// <inheritdoc />
    public partial class AddingPermissionsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermissionModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletionUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletionUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletionUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionSystemModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionSystemId = table.Column<int>(type: "int", nullable: false),
                    PermissionModuleId = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletionUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionSystemModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionSystemModules_PermissionModules_PermissionModuleId",
                        column: x => x.PermissionModuleId,
                        principalTable: "PermissionModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionSystemModules_PermissionSystems_PermissionSystemId",
                        column: x => x.PermissionSystemId,
                        principalTable: "PermissionSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionSystemModuleOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionSystemModuleId = table.Column<int>(type: "int", nullable: false),
                    PermissionOperationId = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletionUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionSystemModuleOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionSystemModuleOperations_PermissionOperations_PermissionOperationId",
                        column: x => x.PermissionOperationId,
                        principalTable: "PermissionOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionSystemModuleOperations_PermissionSystemModules_PermissionSystemModuleId",
                        column: x => x.PermissionSystemModuleId,
                        principalTable: "PermissionSystemModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionSystemModuleOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionSystemModuleOperationId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletionUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionSystemModuleOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissionSystemModuleOperations_PermissionSystemModuleOperations_PermissionSystemModuleOperationId",
                        column: x => x.PermissionSystemModuleOperationId,
                        principalTable: "PermissionSystemModuleOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionSystemModuleOperations_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enabled",
                table: "PermissionModules",
                column: "Enabled",
                filter: "[Enabled] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                table: "PermissionModules",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted_Name_Enabled",
                table: "PermissionModules",
                columns: new[] { "IsDeleted", "Name", "Enabled" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted_Name",
                table: "PermissionOperations",
                columns: new[] { "IsDeleted", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted_Enabled_Guid",
                table: "PermissionSystemModuleOperations",
                columns: new[] { "IsDeleted", "Enabled", "Guid" });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionSystemModuleOperations_PermissionOperationId",
                table: "PermissionSystemModuleOperations",
                column: "PermissionOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionSystemModuleOperations_PermissionSystemModuleId",
                table: "PermissionSystemModuleOperations",
                column: "PermissionSystemModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionSystemModules_PermissionModuleId",
                table: "PermissionSystemModules",
                column: "PermissionModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionSystemModules_PermissionSystemId",
                table: "PermissionSystemModules",
                column: "PermissionSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Enabled",
                table: "PermissionSystems",
                column: "Enabled",
                filter: "[Enabled] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                table: "PermissionSystems",
                column: "IsDeleted",
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted_Name_Enabled",
                table: "PermissionSystems",
                columns: new[] { "IsDeleted", "Name", "Enabled" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionSystemModuleOperations_PermissionSystemModuleOperationId",
                table: "RolePermissionSystemModuleOperations",
                column: "PermissionSystemModuleOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionSystemModuleOperations_RoleId",
                table: "RolePermissionSystemModuleOperations",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissionSystemModuleOperations");

            migrationBuilder.DropTable(
                name: "PermissionSystemModuleOperations");

            migrationBuilder.DropTable(
                name: "PermissionOperations");

            migrationBuilder.DropTable(
                name: "PermissionSystemModules");

            migrationBuilder.DropTable(
                name: "PermissionModules");

            migrationBuilder.DropTable(
                name: "PermissionSystems");
        }
    }
}
