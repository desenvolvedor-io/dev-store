using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Pedidos.Infra.Migrations
{
    public partial class Voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Codigo = table.Column<string>(type: "varchar(100)", nullable: false),
                    Percentual = table.Column<decimal>(nullable: true),
                    ValorDesconto = table.Column<decimal>(nullable: true),
                    Quantidade = table.Column<int>(nullable: false),
                    TipoDesconto = table.Column<int>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataUtilizacao = table.Column<DateTime>(nullable: true),
                    DataValidade = table.Column<DateTime>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false),
                    Utilizado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vouchers");
        }
    }
}
