using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPagoDigital.Migrations
{
    /// <inheritdoc />
    public partial class AgregaEstadoMovimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Movimientos",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Completado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Movimientos");
        }
    }
}
