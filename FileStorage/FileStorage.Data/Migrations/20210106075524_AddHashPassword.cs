using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileStorage.DAL.Migrations
{
    public partial class AddHashPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "HashPassword",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HashPassword", "Login", "Salt" },
                values: new object[] { new byte[] { 164, 181, 67, 172, 129, 233, 242, 122, 51, 24, 59, 199, 61, 53, 157, 214 }, "firstUser", new byte[] { 59, 150, 61, 29, 203, 92, 208, 46, 182, 48, 46, 20, 232, 71, 15, 96 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HashPassword", "Salt" },
                values: new object[] { new byte[] { 172, 255, 180, 33, 194, 94, 224, 118, 223, 194, 114, 112, 233, 43, 18, 162 }, new byte[] { 179, 102, 130, 116, 81, 218, 174, 30, 187, 68, 86, 254, 30, 87, 203, 225 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Login", "Password" },
                values: new object[] { "storageUser", "storagePassword" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "secondPassword");
        }
    }
}
