using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CnbApp.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyDatas",
                columns: table => new
                {
                    CurrencyDataId = table.Column<string>(type: "text", nullable: false),
                    Time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Count = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrencyId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyDatas", x => x.CurrencyDataId);
                    table.ForeignKey(
                        name: "FK_CurrencyDatas_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyDatas_CurrencyId",
                table: "CurrencyDatas",
                column: "CurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyDatas");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
