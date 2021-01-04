using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorage.DAL.Migrations
{
    public partial class AddSecondUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Login", "Password" },
                values: new object[] { "storageUser", "storagePassword" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password" },
                values: new object[] { 2, "secondUser", "secondPassword" });

            migrationBuilder.InsertData(
                table: "Directories",
                columns: new[] { "Id", "Name", "ParentId", "Path", "UserId" },
                values: new object[] { 2, "root", null, "/root", 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Directories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Login", "Password" },
                values: new object[] { "StorageUser", "StoragePassword" });
        }
    }
}
