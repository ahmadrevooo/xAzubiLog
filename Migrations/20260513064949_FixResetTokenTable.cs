using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xAzubiLog.Migrations
{
    /// <inheritdoc />
    public partial class FixResetTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verwendet",
                table: "PasswortResetTokens");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PasswortResetTokens",
                newName: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PasswortResetTokens",
                newName: "Id");

            migrationBuilder.AddColumn<bool>(
                name: "Verwendet",
                table: "PasswortResetTokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
