using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xAzubiLog.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ausbilder",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Abteilung = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ausbilder", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Vorname = table.Column<string>(type: "TEXT", nullable: false),
                    Nachname = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswortHash = table.Column<string>(type: "TEXT", nullable: false),
                    Schule = table.Column<string>(type: "TEXT", nullable: false),
                    Klasse = table.Column<string>(type: "TEXT", nullable: false),
                    Ausbildungsberuf = table.Column<string>(type: "TEXT", nullable: false),
                    Aktiv = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kategorie",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BenutzerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FarbeHex = table.Column<string>(type: "TEXT", nullable: false),
                    Reihenfolge = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorie", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Kategorie_User_BenutzerId",
                        column: x => x.BenutzerId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wochenbericht",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BenutzerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Kalenderwoche = table.Column<int>(type: "INTEGER", nullable: false),
                    Gesamtstunden = table.Column<double>(type: "REAL", nullable: false),
                    Jahr = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Kommentar = table.Column<string>(type: "TEXT", nullable: false),
                    ErstelltAm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wochenbericht", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wochenbericht_User_BenutzerId",
                        column: x => x.BenutzerId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BerichtEintraege",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BenutzerId = table.Column<int>(type: "INTEGER", nullable: false),
                    AusbilderId = table.Column<int>(type: "INTEGER", nullable: true),
                    KategorieId = table.Column<int>(type: "INTEGER", nullable: true),
                    WochenberichtId = table.Column<int>(type: "INTEGER", nullable: false),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tagestyp = table.Column<string>(type: "TEXT", nullable: false),
                    Auftragsnummer = table.Column<string>(type: "TEXT", nullable: true),
                    Titel = table.Column<string>(type: "TEXT", nullable: false),
                    Beschreibung = table.Column<string>(type: "TEXT", nullable: false),
                    Notiz = table.Column<string>(type: "TEXT", nullable: false),
                    Fach = table.Column<string>(type: "TEXT", nullable: true),
                    Startzeit = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Endzeit = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Dauer = table.Column<decimal>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ErstelltAm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GeändertAm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BerichtEintraege", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BerichtEintraege_Ausbilder_AusbilderId",
                        column: x => x.AusbilderId,
                        principalTable: "Ausbilder",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_BerichtEintraege_Kategorie_KategorieId",
                        column: x => x.KategorieId,
                        principalTable: "Kategorie",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_BerichtEintraege_User_BenutzerId",
                        column: x => x.BenutzerId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BerichtEintraege_Wochenbericht_WochenberichtId",
                        column: x => x.WochenberichtId,
                        principalTable: "Wochenbericht",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BerichtEintraege_AusbilderId",
                table: "BerichtEintraege",
                column: "AusbilderId");

            migrationBuilder.CreateIndex(
                name: "IX_BerichtEintraege_BenutzerId",
                table: "BerichtEintraege",
                column: "BenutzerId");

            migrationBuilder.CreateIndex(
                name: "IX_BerichtEintraege_KategorieId",
                table: "BerichtEintraege",
                column: "KategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_BerichtEintraege_WochenberichtId",
                table: "BerichtEintraege",
                column: "WochenberichtId");

            migrationBuilder.CreateIndex(
                name: "IX_Kategorie_BenutzerId",
                table: "Kategorie",
                column: "BenutzerId");

            migrationBuilder.CreateIndex(
                name: "IX_Wochenbericht_BenutzerId",
                table: "Wochenbericht",
                column: "BenutzerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BerichtEintraege");

            migrationBuilder.DropTable(
                name: "Ausbilder");

            migrationBuilder.DropTable(
                name: "Kategorie");

            migrationBuilder.DropTable(
                name: "Wochenbericht");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
