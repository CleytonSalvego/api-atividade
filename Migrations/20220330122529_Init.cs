using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_stc_perfil",
                columns: table => new
                {
                    codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_stc_perfil", x => x.codigo);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_stc_status",
                columns: table => new
                {
                    codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ativo = table.Column<ulong>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_stc_status", x => x.codigo);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_cad_usuario",
                columns: table => new
                {
                    codigo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    senha = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nome = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_perfil = table.Column<int>(type: "INT", nullable: false),
                    ativo = table.Column<ulong>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_cad_usuario", x => x.codigo);
                    table.ForeignKey(
                        name: "FK_Perfil_Usuario",
                        column: x => x.codigo_perfil,
                        principalTable: "tb_stc_perfil",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_cad_atividade",
                columns: table => new
                {
                    codigo = table.Column<long>(type: "BIGINT", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    numero_documento = table.Column<long>(type: "BIGINT", nullable: false),
                    titulo = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descricao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_criacao = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(2022, 3, 30, 9, 25, 29, 395, DateTimeKind.Local).AddTicks(9512)),
                    data_alteracao = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    solicitante = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_status = table.Column<int>(type: "int", nullable: false),
                    data_planejamento = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    codigo_criador = table.Column<long>(type: "BIGINT", nullable: false),
                    UsuarioModelcodigo = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_cad_atividade", x => x.codigo);
                    table.ForeignKey(
                        name: "FK_Atividade_Criador",
                        column: x => x.codigo_criador,
                        principalTable: "tb_cad_usuario",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Atividade_Status",
                        column: x => x.codigo_status,
                        principalTable: "tb_stc_status",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_cad_atividade_tb_cad_usuario_UsuarioModelcodigo",
                        column: x => x.UsuarioModelcodigo,
                        principalTable: "tb_cad_usuario",
                        principalColumn: "codigo");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tb_cad_atividade_codigo_criador",
                table: "tb_cad_atividade",
                column: "codigo_criador");

            migrationBuilder.CreateIndex(
                name: "IX_tb_cad_atividade_codigo_status",
                table: "tb_cad_atividade",
                column: "codigo_status");

            migrationBuilder.CreateIndex(
                name: "IX_tb_cad_atividade_UsuarioModelcodigo",
                table: "tb_cad_atividade",
                column: "UsuarioModelcodigo");

            migrationBuilder.CreateIndex(
                name: "IX_tb_cad_usuario_codigo_perfil",
                table: "tb_cad_usuario",
                column: "codigo_perfil");

            migrationBuilder.CreateIndex(
                name: "IX_usuario",
                table: "tb_cad_usuario",
                column: "usuario",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_cad_atividade");

            migrationBuilder.DropTable(
                name: "tb_cad_usuario");

            migrationBuilder.DropTable(
                name: "tb_stc_status");

            migrationBuilder.DropTable(
                name: "tb_stc_perfil");
        }
    }
}
