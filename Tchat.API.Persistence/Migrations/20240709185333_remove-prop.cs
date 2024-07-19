using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tchat.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ContactQuestion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ContactQuestion",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
