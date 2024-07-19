using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tchat.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class adduserdata2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicturePath",
                table: "AspNetUsers");
        }
    }
}
