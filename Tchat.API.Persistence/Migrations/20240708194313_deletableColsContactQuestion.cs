using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tchat.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deletableColsContactQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ContactQuestion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ContactQuestion",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ContactQuestion");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ContactQuestion");
        }
    }
}
