using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Carrinho.API.Migrations
{
    public partial class Voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Desconto",
                table: "CarrinhoCliente",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "VoucherUtilizado",
                table: "CarrinhoCliente",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VoucherCodigo",
                table: "CarrinhoCliente",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentual",
                table: "CarrinhoCliente",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoDesconto",
                table: "CarrinhoCliente",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorDesconto",
                table: "CarrinhoCliente",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desconto",
                table: "CarrinhoCliente");

            migrationBuilder.DropColumn(
                name: "VoucherUtilizado",
                table: "CarrinhoCliente");

            migrationBuilder.DropColumn(
                name: "VoucherCodigo",
                table: "CarrinhoCliente");

            migrationBuilder.DropColumn(
                name: "Percentual",
                table: "CarrinhoCliente");

            migrationBuilder.DropColumn(
                name: "TipoDesconto",
                table: "CarrinhoCliente");

            migrationBuilder.DropColumn(
                name: "ValorDesconto",
                table: "CarrinhoCliente");
        }
    }
}
