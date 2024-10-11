using Microsoft.EntityFrameworkCore.Migrations;

namespace BiPro_Analytics.Migrations
{
    public partial class prueba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diagnotico",
                table: "Diagnosticos");

            migrationBuilder.AddColumn<string>(
                name: "Diagnostico",
                table: "Diagnosticos",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diagnostico",
                table: "Diagnosticos");

            migrationBuilder.AddColumn<string>(
                name: "Diagnotico",
                table: "Diagnosticos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
