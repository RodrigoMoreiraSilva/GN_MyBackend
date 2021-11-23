using Microsoft.EntityFrameworkCore.Migrations;

namespace GestaoUnica_backend.Migrations
{
    public partial class Services_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_GrupoEmpresa_GrupoEmpresaId",
                table: "Empresas");

            migrationBuilder.AddColumn<bool>(
                name: "ActiveDirectoryAuth",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PasswordExpired",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "GrupoEmpresaId",
                table: "Empresas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_GrupoEmpresa_GrupoEmpresaId",
                table: "Empresas",
                column: "GrupoEmpresaId",
                principalTable: "GrupoEmpresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_GrupoEmpresa_GrupoEmpresaId",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "ActiveDirectoryAuth",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PasswordExpired",
                table: "Usuarios");

            migrationBuilder.AlterColumn<int>(
                name: "GrupoEmpresaId",
                table: "Empresas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_GrupoEmpresa_GrupoEmpresaId",
                table: "Empresas",
                column: "GrupoEmpresaId",
                principalTable: "GrupoEmpresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
