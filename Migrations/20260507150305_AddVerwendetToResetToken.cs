using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xAzubiLog.Migrations
{
    /// <inheritdoc />
    public partial class AddVerwendetToResetToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "PasswortResetTokens");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PasswortResetTokens",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PasswortResetTokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PasswortResetTokens");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PasswortResetTokens",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PasswortResetTokens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
