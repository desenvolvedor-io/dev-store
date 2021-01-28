using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Identidade.API.Migrations
{
    public partial class SecKyes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SecurityKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Parameters = table.Column<string>(nullable: true),
                    KeyId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Algorithm = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityKeys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecurityKeys");
        }
    }
}
