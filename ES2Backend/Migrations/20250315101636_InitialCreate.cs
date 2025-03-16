using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ES2Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tarefa",
                columns: table => new
                {
                    idTarefa = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    dataInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    precoHora = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tarefa_pkey", x => x.idTarefa);
                });

            migrationBuilder.CreateTable(
                name: "Utilizador",
                columns: table => new
                {
                    idUtilizador = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    numHoras = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Utilizador_pkey", x => x.idUtilizador);
                });

            migrationBuilder.CreateTable(
                name: "Projeto",
                columns: table => new
                {
                    idProjeto = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nomeCliente = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    precoHora = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    idUtilizador = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Projeto_pkey", x => x.idProjeto);
                    table.ForeignKey(
                        name: "FK_idUtilizador",
                        column: x => x.idUtilizador,
                        principalTable: "Utilizador",
                        principalColumn: "idUtilizador");
                });

            migrationBuilder.CreateTable(
                name: "TarefaUtilizador",
                columns: table => new
                {
                    idUtilizador = table.Column<int>(type: "integer", nullable: false),
                    idTarefa = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TarefaUtilizador_pkey", x => new { x.idUtilizador, x.idTarefa });
                    table.ForeignKey(
                        name: "FK_idTarefa",
                        column: x => x.idTarefa,
                        principalTable: "Tarefa",
                        principalColumn: "idTarefa");
                    table.ForeignKey(
                        name: "FK_idUtilizador",
                        column: x => x.idUtilizador,
                        principalTable: "Utilizador",
                        principalColumn: "idUtilizador");
                });

            migrationBuilder.CreateTable(
                name: "Membro",
                columns: table => new
                {
                    idMembro = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    estado = table.Column<bool>(type: "boolean", nullable: false),
                    idUtilizador = table.Column<int>(type: "integer", nullable: false),
                    idProjeto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Membro_pkey", x => x.idMembro);
                    table.ForeignKey(
                        name: "FK_idProjeto",
                        column: x => x.idProjeto,
                        principalTable: "Projeto",
                        principalColumn: "idProjeto");
                    table.ForeignKey(
                        name: "FK_idUtilizador",
                        column: x => x.idUtilizador,
                        principalTable: "Utilizador",
                        principalColumn: "idUtilizador");
                });

            migrationBuilder.CreateTable(
                name: "TarefaProjeto",
                columns: table => new
                {
                    idProjeto = table.Column<int>(type: "integer", nullable: false),
                    idTarefa = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TarefaProjeto_pkey", x => new { x.idProjeto, x.idTarefa });
                    table.ForeignKey(
                        name: "FK_idProjeto",
                        column: x => x.idProjeto,
                        principalTable: "Projeto",
                        principalColumn: "idProjeto");
                    table.ForeignKey(
                        name: "FK_idTarefa",
                        column: x => x.idTarefa,
                        principalTable: "Tarefa",
                        principalColumn: "idTarefa");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Membro_idProjeto",
                table: "Membro",
                column: "idProjeto");

            migrationBuilder.CreateIndex(
                name: "IX_Membro_idUtilizador",
                table: "Membro",
                column: "idUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_idUtilizador",
                table: "Projeto",
                column: "idUtilizador");

            migrationBuilder.CreateIndex(
                name: "IX_TarefaProjeto_idTarefa",
                table: "TarefaProjeto",
                column: "idTarefa");

            migrationBuilder.CreateIndex(
                name: "IX_TarefaUtilizador_idTarefa",
                table: "TarefaUtilizador",
                column: "idTarefa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Membro");

            migrationBuilder.DropTable(
                name: "TarefaProjeto");

            migrationBuilder.DropTable(
                name: "TarefaUtilizador");

            migrationBuilder.DropTable(
                name: "Projeto");

            migrationBuilder.DropTable(
                name: "Tarefa");

            migrationBuilder.DropTable(
                name: "Utilizador");
        }
    }
}
