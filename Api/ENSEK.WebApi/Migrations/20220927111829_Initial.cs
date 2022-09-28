using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENSEK.WebApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalRecords = table.Column<int>(type: "int", nullable: false),
                    Successful = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeterReads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    MeterReadingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MeterReadValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReads", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1234, "Freya", "Test" },
                    { 1239, "Noddy", "Test" },
                    { 1240, "Archie", "Test" },
                    { 1241, "Lara", "Test" },
                    { 1242, "Tim", "Test" },
                    { 1243, "Graham", "Test" },
                    { 1244, "Tony", "Test" },
                    { 1245, "Neville", "Test" },
                    { 1246, "Jo", "Test" },
                    { 1247, "Jim", "Test" },
                    { 1248, "Pam", "Test" },
                    { 2233, "Barry", "Test" },
                    { 2344, "Tommy", "Test" },
                    { 2345, "Jerry", "Test" },
                    { 2346, "Ollie", "Test" },
                    { 2347, "Tara", "Test" },
                    { 2348, "Tammy", "Test" },
                    { 2349, "Simon", "Test" },
                    { 2350, "Colin", "Test" },
                    { 2351, "Gladys", "Test" },
                    { 2352, "Greg", "Test" },
                    { 2353, "Tony", "Test" },
                    { 2355, "Arthur", "Test" },
                    { 2356, "Craig", "Test" },
                    { 4534, "JOSH", "Test" },
                    { 6776, "Laura", "Test" },
                    { 8766, "Sally", "Test" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterReads_AccountId",
                table: "MeterReads",
                column: "AccountId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropTable(
                name: "MeterReads");
        }
    }
}
