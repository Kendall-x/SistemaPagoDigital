using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPagoDigital.Migrations
{
    /// <inheritdoc />
    public partial class AgregaIndiceUnicoPagoCobro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Cobros_CobroId",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_CobroId",
                table: "Pagos");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Cobros",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CobroId",
                table: "Pagos",
                column: "CobroId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Cobros_CobroId",
                table: "Pagos",
                column: "CobroId",
                principalTable: "Cobros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pagos_CobroId",
                table: "Pagos");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Cobros",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CobroId",
                table: "Pagos",
                column: "CobroId");
        }
    }
}
