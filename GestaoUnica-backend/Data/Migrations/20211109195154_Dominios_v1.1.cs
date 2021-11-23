using Microsoft.EntityFrameworkCore.Migrations;

namespace GestaoUnica_backend.Migrations
{
    public partial class Dominios_v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Regras_RegrasId",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "RegrasId",
                table: "Logs",
                newName: "RegraId");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_RegrasId",
                table: "Logs",
                newName: "IX_Logs_RegraId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Regras_RegraId",
                table: "Logs",
                column: "RegraId",
                principalTable: "Regras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Regras_RegraId",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "RegraId",
                table: "Logs",
                newName: "RegrasId");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_RegraId",
                table: "Logs",
                newName: "IX_Logs_RegrasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Regras_RegrasId",
                table: "Logs",
                column: "RegrasId",
                principalTable: "Regras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
