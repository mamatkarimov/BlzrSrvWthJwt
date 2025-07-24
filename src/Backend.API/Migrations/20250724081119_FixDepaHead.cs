using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.API.Migrations
{
    /// <inheritdoc />
    public partial class FixDepaHead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_StaffProfiles_HeadDoctorId",
                table: "Departments");

            migrationBuilder.AlterColumn<int>(
                name: "HeadDoctorId",
                table: "Departments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "FK_Departments_StaffProfiles_HeadDoctorId",
                table: "Departments",
                column: "HeadDoctorId",
                principalTable: "StaffProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_StaffProfiles_HeadDoctorId",
                table: "Departments");

            migrationBuilder.AlterColumn<int>(
                name: "HeadDoctorId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c784d6e7-4424-4fe1-a1bb-b03c6a9a26cb",
                column: "RegisterDate",
                value: new DateTime(2025, 7, 24, 13, 5, 49, 481, DateTimeKind.Local).AddTicks(7745));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f0dccee8-a3e1-45f8-9bb7-f7e7decebd09",
                column: "RegisterDate",
                value: new DateTime(2025, 7, 24, 13, 5, 49, 481, DateTimeKind.Local).AddTicks(7765));

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_StaffProfiles_HeadDoctorId",
                table: "Departments",
                column: "HeadDoctorId",
                principalTable: "StaffProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
