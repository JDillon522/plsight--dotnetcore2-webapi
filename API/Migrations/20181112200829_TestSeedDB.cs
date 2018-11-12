using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class TestSeedDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Lets see if this works", "Seeded City Name" });

            migrationBuilder.InsertData(
                table: "Points",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[] { 1, 1, "Test 1", "First Point" });

            migrationBuilder.InsertData(
                table: "Points",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[] { 2, 1, "Test 2", "Second Point" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Points",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Points",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
