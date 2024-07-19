using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tchat.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTchatIpPropertiesToMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TchatIp",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TchatIp",
                table: "Messages");
        }
    }
}
