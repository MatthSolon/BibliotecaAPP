using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAlunoAndEmprestimo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlunoID",
                table: "Livros",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataDevolucao",
                table: "Livros",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataDevolucaoPrevista",
                table: "Livros",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEmprestimo",
                table: "Livros",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Alunos",
                columns: table => new
                {
                    AlunoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.AlunoID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livros_AlunoID",
                table: "Livros",
                column: "AlunoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Alunos_AlunoID",
                table: "Livros",
                column: "AlunoID",
                principalTable: "Alunos",
                principalColumn: "AlunoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Alunos_AlunoID",
                table: "Livros");

            migrationBuilder.DropTable(
                name: "Alunos");

            migrationBuilder.DropIndex(
                name: "IX_Livros_AlunoID",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "AlunoID",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "DataDevolucao",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "DataDevolucaoPrevista",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "DataEmprestimo",
                table: "Livros");
        }
    }
}
