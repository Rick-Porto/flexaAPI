using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flexa.Infra.Migrations
{
    /// <inheritdoc />
    public partial class fixAppName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_screens_apps_AppId",
                table: "screens");

            migrationBuilder.DropTable(
                name: "apps");

            migrationBuilder.RenameColumn(
                name: "AppId",
                table: "screens",
                newName: "AppEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_screens_AppId",
                table: "screens",
                newName: "IX_screens_AppEntityId");

            migrationBuilder.CreateTable(
                name: "app_entities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_entities", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_screens_app_entities_AppEntityId",
                table: "screens",
                column: "AppEntityId",
                principalTable: "app_entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_screens_app_entities_AppEntityId",
                table: "screens");

            migrationBuilder.DropTable(
                name: "app_entities");

            migrationBuilder.RenameColumn(
                name: "AppEntityId",
                table: "screens",
                newName: "AppId");

            migrationBuilder.RenameIndex(
                name: "IX_screens_AppEntityId",
                table: "screens",
                newName: "IX_screens_AppId");

            migrationBuilder.CreateTable(
                name: "apps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apps", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_screens_apps_AppId",
                table: "screens",
                column: "AppId",
                principalTable: "apps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
