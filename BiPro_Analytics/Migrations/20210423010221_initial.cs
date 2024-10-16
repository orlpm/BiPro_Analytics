﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BiPro_Analytics.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diagnosticos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diagnotico = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosticos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    IdEmpresa = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false),
                    RazonSocial = table.Column<string>(maxLength: 50, nullable: false),
                    RFC = table.Column<string>(maxLength: 13, nullable: false),
                    ActividadPrincipal = table.Column<string>(maxLength: 30, nullable: false),
                    Sector = table.Column<string>(maxLength: 40, nullable: false),
                    Calle = table.Column<string>(maxLength: 40, nullable: false),
                    NumeroExt = table.Column<string>(maxLength: 5, nullable: false),
                    NumeroInt = table.Column<string>(maxLength: 4, nullable: true),
                    Colonia = table.Column<string>(maxLength: 100, nullable: true),
                    Ciudad = table.Column<string>(maxLength: 50, nullable: true),
                    Municipio = table.Column<string>(maxLength: 50, nullable: true),
                    Estado = table.Column<string>(maxLength: 30, nullable: false),
                    CP = table.Column<string>(maxLength: 6, nullable: false),
                    Aministrador = table.Column<string>(maxLength: 50, nullable: false),
                    Puesto = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: false),
                    Correo = table.Column<string>(maxLength: 60, nullable: false),
                    CantidadEmpleados = table.Column<int>(nullable: false),
                    SueldoMinimo = table.Column<decimal>(type: "decimal(8, 2)", nullable: false),
                    SueldoMaximo = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    SueldoPromedio = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    NumeroSucursales = table.Column<int>(nullable: false),
                    Comedor = table.Column<bool>(nullable: false),
                    TransporteTrabajadores = table.Column<bool>(nullable: false),
                    ServicioMedico = table.Column<bool>(nullable: false),
                    SGMM = table.Column<bool>(nullable: false),
                    TrabajadoresConSGMM = table.Column<int>(nullable: false),
                    NombreAseguradora = table.Column<string>(nullable: true),
                    NombreAgenteSeguros = table.Column<string>(nullable: true),
                    HorasLaborales = table.Column<int>(nullable: false),
                    DiasLaborales = table.Column<int>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    CodigoEmpresa = table.Column<string>(nullable: true),
                    DescartarUnidades = table.Column<bool>(nullable: false),
                    DescartarAreas = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.IdEmpresa);
                });

            migrationBuilder.CreateTable(
                name: "SintomasCovid",
                columns: table => new
                {
                    IdSintoma = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sintoma = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SintomasCovid", x => x.IdSintoma);
                });

            migrationBuilder.CreateTable(
                name: "TiposPruebas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDePrueba = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposPruebas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ubicacion",
                columns: table => new
                {
                    Identificador = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicacion", x => x.Identificador);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosEmpresas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<Guid>(nullable: false),
                    IdEmpresa = table.Column<int>(nullable: false),
                    Rol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosEmpresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosTrabajadores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    TrabajadorId = table.Column<int>(nullable: false),
                    CodigoEmpresa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosTrabajadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false),
                    IdEmpresa = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Unidades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false),
                    IdEmpresa = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unidades_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReporteContagio",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoPrueba = table.Column<string>(nullable: false),
                    NumeroPruebas = table.Column<int>(nullable: false),
                    Positivos = table.Column<int>(nullable: false),
                    Negativos = table.Column<int>(nullable: false),
                    FechaInicio = table.Column<DateTime>(nullable: false),
                    FechaFin = table.Column<DateTime>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    IdEmpresa = table.Column<int>(nullable: true),
                    IdUnidad = table.Column<int>(nullable: true),
                    IdArea = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReporteContagio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReporteContagio_Areas_IdArea",
                        column: x => x.IdArea,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReporteContagio_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReporteContagio_Unidades_IdUnidad",
                        column: x => x.IdUnidad,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trabajadores",
                columns: table => new
                {
                    IdTrabajador = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(nullable: false),
                    Genero = table.Column<string>(maxLength: 10, nullable: false),
                    Telefono = table.Column<string>(nullable: false),
                    Correo = table.Column<string>(maxLength: 60, nullable: false),
                    Calle = table.Column<string>(maxLength: 80, nullable: false),
                    NumeroExt = table.Column<string>(maxLength: 5, nullable: false),
                    NumeroInt = table.Column<string>(maxLength: 8, nullable: false),
                    CP = table.Column<string>(maxLength: 6, nullable: false),
                    Estado = table.Column<string>(maxLength: 30, nullable: false),
                    Municipio = table.Column<string>(maxLength: 50, nullable: false),
                    Ciudad = table.Column<string>(maxLength: 40, nullable: false),
                    FechaIngreso = table.Column<DateTime>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    NombreUnidad = table.Column<string>(nullable: true),
                    NombreArea = table.Column<string>(nullable: true),
                    IdEmpresa = table.Column<int>(nullable: true),
                    NombreEmpresa = table.Column<string>(nullable: true),
                    IdUnidad = table.Column<int>(nullable: true),
                    IdArea = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajadores", x => x.IdTrabajador);
                    table.ForeignKey(
                        name: "FK_Trabajadores_Areas_IdArea",
                        column: x => x.IdArea,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trabajadores_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trabajadores_Unidades_IdUnidad",
                        column: x => x.IdUnidad,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactoresRiesgos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diabetes = table.Column<bool>(nullable: false),
                    Hipertension = table.Column<bool>(nullable: false),
                    Asma = table.Column<bool>(nullable: false),
                    SobrePeso = table.Column<bool>(nullable: false),
                    Obesidad = table.Column<bool>(nullable: false),
                    EnfermedadAutoinmune = table.Column<bool>(nullable: false),
                    EnfermedadCorazon = table.Column<bool>(nullable: false),
                    EPOC = table.Column<bool>(nullable: false),
                    Embarazo = table.Column<bool>(nullable: false),
                    Cancer = table.Column<bool>(nullable: false),
                    Tabaquismo = table.Column<bool>(nullable: false),
                    ConsumoAlcohol = table.Column<bool>(nullable: false),
                    FarmacosDrogas = table.Column<bool>(nullable: false),
                    NoPersonasCasa = table.Column<int>(nullable: false),
                    NoPersonasTerreno = table.Column<int>(nullable: false),
                    TipoCasa = table.Column<string>(maxLength: 20, nullable: false),
                    TipoTransporte = table.Column<string>(maxLength: 100, nullable: false),
                    EspacioTrabajo = table.Column<string>(maxLength: 120, nullable: false),
                    TipoVentilacion = table.Column<string>(maxLength: 120, nullable: false),
                    ContactoLaboral = table.Column<string>(maxLength: 100, nullable: false),
                    TiempoContacto = table.Column<string>(maxLength: 15, nullable: false),
                    FechaHoraRegistro = table.Column<DateTime>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoresRiesgos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactoresRiesgos_Trabajadores_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Incapacidades",
                columns: table => new
                {
                    IdIncapacidad = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaHoraInicio = table.Column<DateTime>(nullable: false),
                    FechaHoraFin = table.Column<DateTime>(nullable: true),
                    MotivoIncapacidad = table.Column<string>(nullable: false),
                    TipoIncapacidad = table.Column<string>(nullable: true),
                    SeEncuentraEn = table.Column<string>(nullable: true),
                    IdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incapacidades", x => x.IdIncapacidad);
                    table.ForeignKey(
                        name: "FK_Incapacidades_Trabajadores_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pruebas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaDiagnostico = table.Column<DateTime>(nullable: false),
                    Lugar = table.Column<string>(maxLength: 40, nullable: false),
                    TipoPrueba = table.Column<string>(maxLength: 45, nullable: true),
                    DiagnosticoCovid = table.Column<string>(nullable: false),
                    FechaHoraRegistro = table.Column<DateTime>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: true),
                    UbicacionId = table.Column<int>(nullable: true),
                    IdUnidad = table.Column<int>(nullable: true),
                    IdArea = table.Column<int>(nullable: true),
                    DiagnosticoCovidId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pruebas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pruebas_Diagnosticos_DiagnosticoCovidId",
                        column: x => x.DiagnosticoCovidId,
                        principalTable: "Diagnosticos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pruebas_Areas_IdArea",
                        column: x => x.IdArea,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pruebas_Trabajadores_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pruebas_Unidades_IdUnidad",
                        column: x => x.IdUnidad,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pruebas_Ubicacion_UbicacionId",
                        column: x => x.UbicacionId,
                        principalTable: "Ubicacion",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PruebasInternas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaDiagnostico = table.Column<DateTime>(nullable: false),
                    Lugar = table.Column<string>(maxLength: 40, nullable: false),
                    TipoPrueba = table.Column<string>(maxLength: 45, nullable: true),
                    Resultado = table.Column<string>(maxLength: 100, nullable: false),
                    FechaHoraRegistro = table.Column<DateTime>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: true),
                    UbicacionId = table.Column<int>(nullable: true),
                    IdUnidad = table.Column<int>(nullable: true),
                    IdArea = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PruebasInternas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PruebasInternas_Areas_IdArea",
                        column: x => x.IdArea,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PruebasInternas_Trabajadores_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PruebasInternas_Unidades_IdUnidad",
                        column: x => x.IdUnidad,
                        principalTable: "Unidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PruebasInternas_Ubicacion_UbicacionId",
                        column: x => x.UbicacionId,
                        principalTable: "Ubicacion",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reincorporados",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaDiagnostico = table.Column<DateTime>(nullable: false),
                    FechaTerminoIncapacidad = table.Column<DateTime>(nullable: false),
                    FechaRegresoTrabajo = table.Column<DateTime>(nullable: false),
                    DiasIncapacidad = table.Column<int>(nullable: false),
                    EtudiosSecuelasPulmonares = table.Column<bool>(nullable: false),
                    EtudiosSecuelasNoPulmonares = table.Column<bool>(nullable: false),
                    FisicamenteCapacitado = table.Column<bool>(nullable: false),
                    MotivadoTrabajo = table.Column<bool>(nullable: false),
                    MedicoSeguimiento = table.Column<bool>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reincorporados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reincorporados_Trabajadores_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RiesgoContagios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactoCovidCasa = table.Column<bool>(nullable: false),
                    ContactoCovidTrabajo = table.Column<bool>(nullable: false),
                    ContactoCovidFuera = table.Column<bool>(nullable: false),
                    ViajesMultitudes = table.Column<bool>(nullable: false),
                    TosRecurrente = table.Column<bool>(nullable: false),
                    Tos = table.Column<bool>(nullable: false),
                    DificultadRespirar = table.Column<bool>(nullable: false),
                    TempMayor38 = table.Column<bool>(nullable: false),
                    Resfriado = table.Column<bool>(nullable: false),
                    Escalofrios = table.Column<bool>(nullable: false),
                    DolorMuscular = table.Column<bool>(nullable: false),
                    NauseaVomito = table.Column<bool>(nullable: false),
                    Diarrea = table.Column<bool>(nullable: false),
                    Olfatometria = table.Column<string>(maxLength: 20, nullable: true),
                    FechaHoraRegistro = table.Column<DateTime>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiesgoContagios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiesgoContagios_Trabajadores_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeguimientosCovid",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSintoma = table.Column<int>(nullable: false),
                    IdUbicacion = table.Column<int>(nullable: false),
                    FechaSeguimiento = table.Column<DateTime>(nullable: false),
                    FechaHoraRegistro = table.Column<DateTime>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeguimientosCovid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeguimientosCovid_SintomasCovid_IdSintoma",
                        column: x => x.IdSintoma,
                        principalTable: "SintomasCovid",
                        principalColumn: "IdSintoma",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeguimientosCovid_Trabajadores_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeguimientosCovid_Ubicacion_IdUbicacion",
                        column: x => x.IdUbicacion,
                        principalTable: "Ubicacion",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Archivos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreArchivo = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    IdPrueba = table.Column<int>(nullable: false),
                    PruebaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Archivos_Pruebas_PruebaId",
                        column: x => x.PruebaId,
                        principalTable: "Pruebas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Archivos_PruebaId",
                table: "Archivos",
                column: "PruebaId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_IdEmpresa",
                table: "Areas",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FactoresRiesgos_IdTrabajador",
                table: "FactoresRiesgos",
                column: "IdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_Incapacidades_IdTrabajador",
                table: "Incapacidades",
                column: "IdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_DiagnosticoCovidId",
                table: "Pruebas",
                column: "DiagnosticoCovidId");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_IdArea",
                table: "Pruebas",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_IdTrabajador",
                table: "Pruebas",
                column: "IdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_IdUnidad",
                table: "Pruebas",
                column: "IdUnidad");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_UbicacionId",
                table: "Pruebas",
                column: "UbicacionId");

            migrationBuilder.CreateIndex(
                name: "IX_PruebasInternas_IdArea",
                table: "PruebasInternas",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_PruebasInternas_IdTrabajador",
                table: "PruebasInternas",
                column: "IdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_PruebasInternas_IdUnidad",
                table: "PruebasInternas",
                column: "IdUnidad");

            migrationBuilder.CreateIndex(
                name: "IX_PruebasInternas_UbicacionId",
                table: "PruebasInternas",
                column: "UbicacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reincorporados_IdTrabajador",
                table: "Reincorporados",
                column: "IdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteContagio_IdArea",
                table: "ReporteContagio",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteContagio_IdEmpresa",
                table: "ReporteContagio",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteContagio_IdUnidad",
                table: "ReporteContagio",
                column: "IdUnidad");

            migrationBuilder.CreateIndex(
                name: "IX_RiesgoContagios_IdTrabajador",
                table: "RiesgoContagios",
                column: "IdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosCovid_IdSintoma",
                table: "SeguimientosCovid",
                column: "IdSintoma");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosCovid_IdTrabajador",
                table: "SeguimientosCovid",
                column: "IdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosCovid_IdUbicacion",
                table: "SeguimientosCovid",
                column: "IdUbicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_IdArea",
                table: "Trabajadores",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_IdEmpresa",
                table: "Trabajadores",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_IdUnidad",
                table: "Trabajadores",
                column: "IdUnidad");

            migrationBuilder.CreateIndex(
                name: "IX_Unidades_IdEmpresa",
                table: "Unidades",
                column: "IdEmpresa");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Archivos");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FactoresRiesgos");

            migrationBuilder.DropTable(
                name: "Incapacidades");

            migrationBuilder.DropTable(
                name: "PruebasInternas");

            migrationBuilder.DropTable(
                name: "Reincorporados");

            migrationBuilder.DropTable(
                name: "ReporteContagio");

            migrationBuilder.DropTable(
                name: "RiesgoContagios");

            migrationBuilder.DropTable(
                name: "SeguimientosCovid");

            migrationBuilder.DropTable(
                name: "TiposPruebas");

            migrationBuilder.DropTable(
                name: "UsuariosEmpresas");

            migrationBuilder.DropTable(
                name: "UsuariosTrabajadores");

            migrationBuilder.DropTable(
                name: "Pruebas");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SintomasCovid");

            migrationBuilder.DropTable(
                name: "Diagnosticos");

            migrationBuilder.DropTable(
                name: "Trabajadores");

            migrationBuilder.DropTable(
                name: "Ubicacion");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Unidades");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
