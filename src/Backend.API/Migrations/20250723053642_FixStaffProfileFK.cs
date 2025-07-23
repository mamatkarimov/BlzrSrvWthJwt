using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.API.Migrations
{
    /// <inheritdoc />
    public partial class FixStaffProfileFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffProfile_AspNetUsers_UserId1",
                table: "StaffProfile");

            migrationBuilder.DropIndex(
                name: "IX_StaffProfile_UserId1",
                table: "StaffProfile");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "StaffProfile");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "StaffProfile",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c784d6e7-4424-4fe1-a1bb-b03c6a9a26cb",
                column: "RegisterDate",
                value: new DateTime(2025, 7, 23, 10, 36, 42, 455, DateTimeKind.Local).AddTicks(7177));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f0dccee8-a3e1-45f8-9bb7-f7e7decebd09",
                column: "RegisterDate",
                value: new DateTime(2025, 7, 23, 10, 36, 42, 455, DateTimeKind.Local).AddTicks(7200));

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfile_UserId",
                table: "StaffProfile",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffProfile_AspNetUsers_UserId",
                table: "StaffProfile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffProfile_AspNetUsers_UserId",
                table: "StaffProfile");

            migrationBuilder.DropIndex(
                name: "IX_StaffProfile_UserId",
                table: "StaffProfile");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "StaffProfile",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "StaffProfile",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c784d6e7-4424-4fe1-a1bb-b03c6a9a26cb",
                column: "RegisterDate",
                value: new DateTime(2025, 7, 23, 10, 32, 7, 421, DateTimeKind.Local).AddTicks(2657));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f0dccee8-a3e1-45f8-9bb7-f7e7decebd09",
                column: "RegisterDate",
                value: new DateTime(2025, 7, 23, 10, 32, 7, 421, DateTimeKind.Local).AddTicks(2672));

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfile_UserId1",
                table: "StaffProfile",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffProfile_AspNetUsers_UserId1",
                table: "StaffProfile",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
