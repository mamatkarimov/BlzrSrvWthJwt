using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_StaffProfile_CreatedById",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffProfile_AspNetUsers_UserId",
                table: "StaffProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StaffProfile",
                table: "StaffProfile");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "StaffProfile");

            migrationBuilder.RenameTable(
                name: "StaffProfile",
                newName: "StaffProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_StaffProfile_UserId",
                table: "StaffProfiles",
                newName: "IX_StaffProfiles_UserId");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "StaffProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StaffProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StaffProfiles",
                table: "StaffProfiles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symptoms = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointment_StaffProfiles_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "StaffProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadDoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_StaffProfiles_HeadDoctorId",
                        column: x => x.HeadDoctorId,
                        principalTable: "StaffProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordedByID = table.Column<int>(type: "int", nullable: false),
                    HistoryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistory_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalHistory_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalHistory_StaffProfiles_RecordedByID",
                        column: x => x.RecordedByID,
                        principalTable: "StaffProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientQueue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QueueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientQueue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientQueue_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientQueue_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientQueue_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    WardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    GenderSpecific = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wards_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignedTest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignedTest_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignedTest_TestTemplate_TestTemplateId",
                        column: x => x.TestTemplateId,
                        principalTable: "TestTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BedDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BedNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    WardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BedDto_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Beds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    BedNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beds_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedTestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceRange = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResult_AssignedTest_AssignedTestId",
                        column: x => x.AssignedTestId,
                        principalTable: "AssignedTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hospitalization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BedId = table.Column<int>(type: "int", nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DischargeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiagnosisOnAdmission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosisOnDischarge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttendingDoctorId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitalization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hospitalization_Beds_BedId",
                        column: x => x.BedId,
                        principalTable: "Beds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hospitalization_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hospitalization_StaffProfiles_AttendingDoctorId",
                        column: x => x.AttendingDoctorId,
                        principalTable: "StaffProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NurseRound",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NurseId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoundDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BloodPressure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pulse = table.Column<int>(type: "int", nullable: true),
                    RespirationRate = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HospitalizationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseRound", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NurseRound_Hospitalization_HospitalizationId",
                        column: x => x.HospitalizationId,
                        principalTable: "Hospitalization",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NurseRound_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NurseRound_StaffProfiles_NurseId",
                        column: x => x.NurseId,
                        principalTable: "StaffProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientDiet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HospitalizationId = table.Column<int>(type: "int", nullable: false),
                    DietType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientDiet_Hospitalization_HospitalizationId",
                        column: x => x.HospitalizationId,
                        principalTable: "Hospitalization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientDiet_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfiles_DepartmentId",
                table: "StaffProfiles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DoctorId",
                table: "Appointment",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTest_AppointmentId",
                table: "AssignedTest",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTest_TestTemplateId",
                table: "AssignedTest",
                column: "TestTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_BedDto_WardId",
                table: "BedDto",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_WardId",
                table: "Beds",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_HeadDoctorId",
                table: "Departments",
                column: "HeadDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitalization_AttendingDoctorId",
                table: "Hospitalization",
                column: "AttendingDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitalization_BedId",
                table: "Hospitalization",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitalization_PatientId",
                table: "Hospitalization",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_AppointmentId",
                table: "MedicalHistory",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_PatientId",
                table: "MedicalHistory",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistory_RecordedByID",
                table: "MedicalHistory",
                column: "RecordedByID");

            migrationBuilder.CreateIndex(
                name: "IX_NurseRound_HospitalizationId",
                table: "NurseRound",
                column: "HospitalizationId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseRound_NurseId",
                table: "NurseRound",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseRound_PatientId",
                table: "NurseRound",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDiet_HospitalizationId",
                table: "PatientDiet",
                column: "HospitalizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDiet_PatientId",
                table: "PatientDiet",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientQueue_AppointmentId",
                table: "PatientQueue",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientQueue_DepartmentId",
                table: "PatientQueue",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientQueue_PatientId",
                table: "PatientQueue",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResult_AssignedTestId",
                table: "TestResult",
                column: "AssignedTestId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_DepartmentId",
                table: "Wards",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_StaffProfiles_CreatedById",
                table: "Patients",
                column: "CreatedById",
                principalTable: "StaffProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffProfiles_AspNetUsers_UserId",
                table: "StaffProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffProfiles_Departments_DepartmentId",
                table: "StaffProfiles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_StaffProfiles_CreatedById",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffProfiles_AspNetUsers_UserId",
                table: "StaffProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffProfiles_Departments_DepartmentId",
                table: "StaffProfiles");

            migrationBuilder.DropTable(
                name: "BedDto");

            migrationBuilder.DropTable(
                name: "MedicalHistory");

            migrationBuilder.DropTable(
                name: "NurseRound");

            migrationBuilder.DropTable(
                name: "PatientDiet");

            migrationBuilder.DropTable(
                name: "PatientQueue");

            migrationBuilder.DropTable(
                name: "TestResult");

            migrationBuilder.DropTable(
                name: "Hospitalization");

            migrationBuilder.DropTable(
                name: "AssignedTest");

            migrationBuilder.DropTable(
                name: "Beds");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "TestTemplate");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StaffProfiles",
                table: "StaffProfiles");

            migrationBuilder.DropIndex(
                name: "IX_StaffProfiles_DepartmentId",
                table: "StaffProfiles");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "StaffProfiles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StaffProfiles");

            migrationBuilder.RenameTable(
                name: "StaffProfiles",
                newName: "StaffProfile");

            migrationBuilder.RenameIndex(
                name: "IX_StaffProfiles_UserId",
                table: "StaffProfile",
                newName: "IX_StaffProfile_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "StaffProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StaffProfile",
                table: "StaffProfile",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_StaffProfile_CreatedById",
                table: "Patients",
                column: "CreatedById",
                principalTable: "StaffProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffProfile_AspNetUsers_UserId",
                table: "StaffProfile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
