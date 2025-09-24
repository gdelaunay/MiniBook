using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniBookApp.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuteurId",
                table: "Utilisateurs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuteurId",
                table: "Utilisateurs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
