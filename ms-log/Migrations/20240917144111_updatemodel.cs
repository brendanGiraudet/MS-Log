using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ms_log.Migrations
{
    /// <inheritdoc />
    public partial class updatemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApplicationCode",
                table: "Logs",
                newName: "Payload");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationName",
                table: "Logs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationName",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "Payload",
                table: "Logs",
                newName: "ApplicationCode");
        }
    }
}
