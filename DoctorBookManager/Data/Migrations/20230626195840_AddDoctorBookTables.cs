using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorBookManager.Data.Migrations
{
    public partial class AddDoctorBookTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    StartWorkHour = table.Column<int>(type: "int", nullable: false),
                    EndWorkHour = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorAppointmentModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorAppointmentModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorAppointmentModels_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorReviews_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAppointmentModels_DoctorId",
                table: "DoctorAppointmentModels",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorReviews_DoctorId",
                table: "DoctorReviews",
                column: "DoctorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorAppointmentModels");

            migrationBuilder.DropTable(
                name: "DoctorReviews");

            migrationBuilder.DropTable(
                name: "Doctors");
        }
    }
}
