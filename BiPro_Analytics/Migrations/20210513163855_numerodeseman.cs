using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BiPro_Analytics.Migrations
{
    public partial class numerodeseman : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "ReporteContagio");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "ReporteContagio");

            migrationBuilder.AddColumn<int>(
                name: "NumeroSemana",
                table: "ReporteContagio",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroSemana",
                table: "ReporteContagio");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "ReporteContagio",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio",
                table: "ReporteContagio",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
