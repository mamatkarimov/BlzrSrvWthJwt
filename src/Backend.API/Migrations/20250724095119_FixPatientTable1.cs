using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.API.Migrations
{
    /// <inheritdoc />
    public partial class FixPatientTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_StaffProfiles_CreatedById",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c784d6e7-4424-4fe1-a1bb-b03c6a9a26cb",
                column: "RegisterDate",
                value: new DateTime(2025, 7, 24, 14, 51, 19, 543, DateTimeKind.Local).AddTicks(8568));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f0dccee8-a3e1-45f8-9bb7-f7e7decebd09",
                column: "RegisterDate",
                value: new DateTime(2025, 7, 24, 14, 51, 19, 543, DateTimeKind.Local).AddTicks(8584));

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_CreatedById",
                table: "Patients",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_CreatedById",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c784d6e7-4424-4fe1-a1bb-b03c6a9a26cb",
                column: "RegisterDate",
                value: new DateTime(2025, 7, 24, 13, 11, 19, 609, DateTimeKind.Local).AddTicks(2000));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f0dccee8-a3e1-45f8-9bb7-f7e7decebd09",
                column: "RegisterDate",
                value: new DateTime(2025, 7, 24, 13, 11, 19, 609, DateTimeKind.Local).AddTicks(2029));

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_StaffProfiles_CreatedById",
                table: "Patients",
                column: "CreatedById",
                principalTable: "StaffProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
