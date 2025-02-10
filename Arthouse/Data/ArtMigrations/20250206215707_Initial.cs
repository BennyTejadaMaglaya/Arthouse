using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arthouse.Data.ArtMigrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Artworks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Completed = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    ArtTypeID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artworks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Artworks_ArtTypes_ArtTypeID",
                        column: x => x.ArtTypeID,
                        principalTable: "ArtTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_ArtTypeID",
                table: "Artworks",
                column: "ArtTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_Name_Completed_ArtTypeID",
                table: "Artworks",
                columns: new[] { "Name", "Completed", "ArtTypeID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artworks");

            migrationBuilder.DropTable(
                name: "ArtTypes");
        }
    }
}
