using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flexa.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicLink",
                table: "app_entities",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_entities_PublicLink",
                table: "app_entities",
                column: "PublicLink",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_app_entities_PublicLink",
                table: "app_entities");

            migrationBuilder.DropColumn(
                name: "PublicLink",
                table: "app_entities");
        }
    }
}
