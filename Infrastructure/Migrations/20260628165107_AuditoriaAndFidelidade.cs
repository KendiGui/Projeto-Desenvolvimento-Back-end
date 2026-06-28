using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuditoriaAndFidelidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: true),
                    Acao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Entidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntidadeId = table.Column<long>(type: "bigint", nullable: true),
                    DadosAntes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DadosDepois = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditoria_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FidelidadeConta",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ConsentimentoMarketing = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ConsentimentoData = table.Column<DateTime>(type: "datetime2", nullable: true),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FidelidadeConta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FidelidadeConta_Usuario_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FidelidadeHistorico",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<long>(type: "bigint", nullable: false),
                    PedidoId = table.Column<long>(type: "bigint", nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FidelidadeContaId = table.Column<long>(type: "bigint", nullable: true),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FidelidadeHistorico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FidelidadeHistorico_FidelidadeConta_FidelidadeContaId",
                        column: x => x.FidelidadeContaId,
                        principalTable: "FidelidadeConta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FidelidadeHistorico_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FidelidadeHistorico_Usuario_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_UsuarioId",
                table: "Auditoria",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_FidelidadeConta_ClienteId",
                table: "FidelidadeConta",
                column: "ClienteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FidelidadeHistorico_ClienteId",
                table: "FidelidadeHistorico",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_FidelidadeHistorico_FidelidadeContaId",
                table: "FidelidadeHistorico",
                column: "FidelidadeContaId");

            migrationBuilder.CreateIndex(
                name: "IX_FidelidadeHistorico_PedidoId",
                table: "FidelidadeHistorico",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditoria");

            migrationBuilder.DropTable(
                name: "FidelidadeHistorico");

            migrationBuilder.DropTable(
                name: "FidelidadeConta");
        }
    }
}
