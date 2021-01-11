using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorage.DAL.Migrations
{
    public partial class AddComandsInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "CommandsInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommandName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionTime = table.Column<double>(type: "float", nullable: false),
                    ExecutionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandsInfo", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashPassword", "Salt" },
                values: new object[] { new byte[] { 226, 252, 17, 168, 74, 88, 77, 248, 191, 33, 222, 14, 12, 128, 89, 16 }, new byte[] { 62, 193, 98, 82, 150, 44, 165, 228, 244, 239, 234, 238, 118, 133, 88, 145 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashPassword", "Salt" },
                values: new object[] { new byte[] { 154, 2, 158, 187, 30, 228, 94, 35, 44, 63, 105, 134, 214, 110, 7, 241 }, new byte[] { 53, 251, 167, 64, 186, 37, 81, 30, 201, 12, 18, 131, 85, 35, 25, 78 } });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandsInfo");

            migrationBuilder.DropIndex(
                name: "IX_Users_Login",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashPassword", "Salt" },
                values: new object[] { new byte[] { 164, 181, 67, 172, 129, 233, 242, 122, 51, 24, 59, 199, 61, 53, 157, 214 }, new byte[] { 59, 150, 61, 29, 203, 92, 208, 46, 182, 48, 46, 20, 232, 71, 15, 96 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashPassword", "Salt" },
                values: new object[] { new byte[] { 172, 255, 180, 33, 194, 94, 224, 118, 223, 194, 114, 112, 233, 43, 18, 162 }, new byte[] { 179, 102, 130, 116, 81, 218, 174, 30, 187, 68, 86, 254, 30, 87, 203, 225 } });
        }
    }
}
