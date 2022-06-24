using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kairosApp.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Identificacion = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Nombres = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Apellidos = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Unidad = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CorreoAlterno = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Rol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Estado = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InfoSolicitud = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CuentaUsuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Alias = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    PersonaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentaUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentaUsuarios_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudPersonas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SolicitudId = table.Column<int>(type: "INTEGER", nullable: false),
                    PersonaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudPersonas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudPersonas_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudPersonas_Solicitudes_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitudes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioGrupos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CuentaUsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    GrupoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioGrupos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioGrupos_CuentaUsuarios_CuentaUsuarioId",
                        column: x => x.CuentaUsuarioId,
                        principalTable: "CuentaUsuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioGrupos_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 101, "FIEC" });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 102, "FCSH" });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 103, "FCNM" });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 104, "FCV" });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 105, "FADCOM" });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 106, "FIMCP" });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 107, "FIMCBOR" });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 108, "Rectorado" });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 109, "Vinculos" });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 110, "STEM" });

            migrationBuilder.InsertData(
                table: "Personas",
                columns: new[] { "Id", "Apellidos", "CorreoAlterno", "Identificacion", "Nombres", "Rol", "Telefono", "Unidad" },
                values: new object[] { 100, "Salazar Moreira", "chardan1225@hotmail.com", "0940529829", "Carlos Fernando", "Admin", "0990964027", "FIEC" });

            migrationBuilder.InsertData(
                table: "Personas",
                columns: new[] { "Id", "Apellidos", "CorreoAlterno", "Identificacion", "Nombres", "Rol", "Telefono", "Unidad" },
                values: new object[] { 101, "Banchon Chavez", "melanie1998@hotmail.com", "0925153212", "Melanie Dayanna", "Estudiante", "0987654321", "FIEC" });

            migrationBuilder.InsertData(
                table: "Personas",
                columns: new[] { "Id", "Apellidos", "CorreoAlterno", "Identificacion", "Nombres", "Rol", "Telefono", "Unidad" },
                values: new object[] { 102, "Pachar Duche", "melissa_pachar@hotmail.com", "09876543231", "Melissa Roxanna", "Estudiante", "0978791998", "Rectorado" });

            migrationBuilder.InsertData(
                table: "Solicitudes",
                columns: new[] { "Id", "Estado", "FechaCreacion", "InfoSolicitud" },
                values: new object[] { 101, "No Revisado", new DateTime(2022, 1, 11, 0, 28, 8, 804, DateTimeKind.Local).AddTicks(5660), "{\"nombres\":\"Fernando Carlos\",\"apellidos\":\"Moreira Salazar\",\"identificacion\":\"0912654798\",\"actividad\":\"Estudiante\",\"unidad\":\"FIEC\",\"telefono\":\"0997063143\",\"correo\":\"fernandomoreira@gmail.com\",\"alias_sugerido\":\"fernando.moreira\",\"usuario_sugerido\":\"fermorsa\"}" });

            migrationBuilder.InsertData(
                table: "Solicitudes",
                columns: new[] { "Id", "Estado", "FechaCreacion", "InfoSolicitud" },
                values: new object[] { 102, "No Revisado", new DateTime(2022, 1, 11, 0, 28, 8, 804, DateTimeKind.Local).AddTicks(5823), "{\"nombres\":\"Rosa Cristina\",\"apellidos\":\"Alvarado Castillo\",\"identificacion\":\"0914365487\",\"actividad\":\"Estudiante\",\"unidad\":\"FIMCP\",\"telefono\":\"0991193877\",\"correo\":\"rosita_alvarado@gmail.com\",\"alias_sugerido\":\"rosa.cristina\",\"usuario_sugerido\":\"rosaalva\"}" });

            migrationBuilder.InsertData(
                table: "CuentaUsuarios",
                columns: new[] { "Id", "Alias", "IsActive", "PersonaId", "Username" },
                values: new object[] { 101, "carlos.salazar", true, 100, "carfesal" });

            migrationBuilder.InsertData(
                table: "CuentaUsuarios",
                columns: new[] { "Id", "Alias", "IsActive", "PersonaId", "Username" },
                values: new object[] { 102, "melanie.banchon", true, 101, "meldaban" });

            migrationBuilder.InsertData(
                table: "CuentaUsuarios",
                columns: new[] { "Id", "Alias", "IsActive", "PersonaId", "Username" },
                values: new object[] { 103, "melissa.pachar", true, 102, "melroxan" });

            migrationBuilder.InsertData(
                table: "UsuarioGrupos",
                columns: new[] { "Id", "CuentaUsuarioId", "GrupoId" },
                values: new object[] { 100, 101, 101 });

            migrationBuilder.InsertData(
                table: "UsuarioGrupos",
                columns: new[] { "Id", "CuentaUsuarioId", "GrupoId" },
                values: new object[] { 101, 102, 103 });

            migrationBuilder.InsertData(
                table: "UsuarioGrupos",
                columns: new[] { "Id", "CuentaUsuarioId", "GrupoId" },
                values: new object[] { 102, 102, 104 });

            migrationBuilder.InsertData(
                table: "UsuarioGrupos",
                columns: new[] { "Id", "CuentaUsuarioId", "GrupoId" },
                values: new object[] { 103, 103, 101 });

            migrationBuilder.InsertData(
                table: "UsuarioGrupos",
                columns: new[] { "Id", "CuentaUsuarioId", "GrupoId" },
                values: new object[] { 104, 103, 110 });

            migrationBuilder.InsertData(
                table: "UsuarioGrupos",
                columns: new[] { "Id", "CuentaUsuarioId", "GrupoId" },
                values: new object[] { 105, 102, 109 });

            migrationBuilder.CreateIndex(
                name: "IX_CuentaUsuarios_PersonaId",
                table: "CuentaUsuarios",
                column: "PersonaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudPersonas_PersonaId",
                table: "SolicitudPersonas",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudPersonas_SolicitudId",
                table: "SolicitudPersonas",
                column: "SolicitudId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioGrupos_CuentaUsuarioId",
                table: "UsuarioGrupos",
                column: "CuentaUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioGrupos_GrupoId",
                table: "UsuarioGrupos",
                column: "GrupoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudPersonas");

            migrationBuilder.DropTable(
                name: "UsuarioGrupos");

            migrationBuilder.DropTable(
                name: "Solicitudes");

            migrationBuilder.DropTable(
                name: "CuentaUsuarios");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "Personas");
        }
    }
}
