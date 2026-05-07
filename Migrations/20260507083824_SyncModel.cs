using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xAzubiLog.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BerichtEintrag_Ausbilder_AusbilderId",
                table: "BerichtEintrag");

            migrationBuilder.DropForeignKey(
                name: "FK_BerichtEintrag_Kategorie_KategorieId",
                table: "BerichtEintrag");

            migrationBuilder.DropForeignKey(
                name: "FK_BerichtEintrag_User_BenutzerId",
                table: "BerichtEintrag");

            migrationBuilder.DropForeignKey(
                name: "FK_BerichtEintrag_Wochenbericht_WochenberichtId",
                table: "BerichtEintrag");

            migrationBuilder.DropForeignKey(
                name: "FK_Kategorie_User_BenutzerId",
                table: "Kategorie");

            migrationBuilder.DropForeignKey(
                name: "FK_Wochenbericht_User_BenutzerId",
                table: "Wochenbericht");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wochenbericht",
                table: "Wochenbericht");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kategorie",
                table: "Kategorie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BerichtEintrag",
                table: "BerichtEintrag");

            migrationBuilder.RenameTable(
                name: "Wochenbericht",
                newName: "Wochenberichte");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Kategorie",
                newName: "Kategorien");

            migrationBuilder.RenameTable(
                name: "BerichtEintrag",
                newName: "BerichtEintraege");

            migrationBuilder.RenameIndex(
                name: "IX_Wochenbericht_BenutzerId",
                table: "Wochenberichte",
                newName: "IX_Wochenberichte_BenutzerId");

            migrationBuilder.RenameIndex(
                name: "IX_Kategorie_BenutzerId",
                table: "Kategorien",
                newName: "IX_Kategorien_BenutzerId");

            migrationBuilder.RenameIndex(
                name: "IX_BerichtEintrag_WochenberichtId",
                table: "BerichtEintraege",
                newName: "IX_BerichtEintraege_WochenberichtId");

            migrationBuilder.RenameIndex(
                name: "IX_BerichtEintrag_KategorieId",
                table: "BerichtEintraege",
                newName: "IX_BerichtEintraege_KategorieId");

            migrationBuilder.RenameIndex(
                name: "IX_BerichtEintrag_BenutzerId",
                table: "BerichtEintraege",
                newName: "IX_BerichtEintraege_BenutzerId");

            migrationBuilder.RenameIndex(
                name: "IX_BerichtEintrag_AusbilderId",
                table: "BerichtEintraege",
                newName: "IX_BerichtEintraege_AusbilderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wochenberichte",
                table: "Wochenberichte",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kategorien",
                table: "Kategorien",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BerichtEintraege",
                table: "BerichtEintraege",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BerichtEintraege_Ausbilder_AusbilderId",
                table: "BerichtEintraege",
                column: "AusbilderId",
                principalTable: "Ausbilder",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BerichtEintraege_Kategorien_KategorieId",
                table: "BerichtEintraege",
                column: "KategorieId",
                principalTable: "Kategorien",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BerichtEintraege_Users_BenutzerId",
                table: "BerichtEintraege",
                column: "BenutzerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BerichtEintraege_Wochenberichte_WochenberichtId",
                table: "BerichtEintraege",
                column: "WochenberichtId",
                principalTable: "Wochenberichte",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kategorien_Users_BenutzerId",
                table: "Kategorien",
                column: "BenutzerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wochenberichte_Users_BenutzerId",
                table: "Wochenberichte",
                column: "BenutzerId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
