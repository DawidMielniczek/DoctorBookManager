using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorBookManager.Data.Migrations
{
    public partial class AddPropertCityToDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Doctors",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Doctors");
        }
    }
}
