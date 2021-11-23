using Microsoft.EntityFrameworkCore.Migrations;

namespace GestaoUnica_backend.Migrations
{
    public partial class Services_v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumTentativasAcesso",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumTentativasAcesso",
                table: "Usuarios");
        }
    }
}
