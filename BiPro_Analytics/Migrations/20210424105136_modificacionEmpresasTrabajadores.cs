using Microsoft.EntityFrameworkCore.Migrations;

namespace BiPro_Analytics.Migrations
{
    public partial class modificacionEmpresasTrabajadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreArea",
                table: "Trabajadores");

            migrationBuilder.DropColumn(
                name: "NombreEmpresa",
                table: "Trabajadores");

            migrationBuilder.DropColumn(
                name: "NombreUnidad",
                table: "Trabajadores");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroInt",
                table: "Trabajadores",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumeroInt",
                table: "Trabajadores",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreArea",
                table: "Trabajadores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreEmpresa",
                table: "Trabajadores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreUnidad",
                table: "Trabajadores",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
