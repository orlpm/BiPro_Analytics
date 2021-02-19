using System;
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
                name: "Empresas",
                columns: table => new
                {
                    IdEmpresa = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false),
                    RazonSocial = table.Column<string>(maxLength: 50, nullable: false),
                    RFC = table.Column<string>(maxLength: 12, nullable: false),
                    Aministrador = table.Column<string>(maxLength: 50, nullable: false),
                    Puesto = table.Column<string>(nullable: true),
                    Giro = table.Column<string>(maxLength: 40, nullable: false),
                    SubGiro = table.Column<string>(maxLength: 30, nullable: true),
                    Seccion = table.Column<string>(maxLength: 30, nullable: true),
                    Telefono = table.Column<string>(nullable: false),
                    Correo = table.Column<string>(nullable: false),
                    Calle = table.Column<string>(maxLength: 40, nullable: false),
                    NumeroExt = table.Column<string>(maxLength: 5, nullable: false),
                    NumeroInt = table.Column<string>(maxLength: 4, nullable: false),
                    Ciudad = table.Column<string>(maxLength: 40, nullable: false),
                    Estado = table.Column<string>(maxLength: 30, nullable: false),
                    CP = table.Column<string>(maxLength: 6, nullable: false),
                    CantEmpleados = table.Column<int>(nullable: false),
                    MinSueldo = table.Column<decimal>(type: "decimal(8, 2)", nullable: false),
                    MaxSueldo = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    HorasLaborales = table.Column<int>(nullable: false),
                    DiasLaborales = table.Column<int>(nullable: false),
                    CodigoEmpresa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.IdEmpresa);
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
                    Name = table.Column<string>(nullable: true),
                    IdEmpresa = table.Column<int>(nullable: false),
                    EmpresaIdEmpresa = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_Empresas_EmpresaIdEmpresa",
                        column: x => x.EmpresaIdEmpresa,
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
                    Semana = table.Column<int>(nullable: false),
                    Anio = table.Column<int>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: false),
                    PositivosSemPCR = table.Column<int>(nullable: false),
                    PositivosSemLG = table.Column<int>(nullable: false),
                    PositivosSemAntigeno = table.Column<int>(nullable: false),
                    PositivosSemTAC = table.Column<int>(nullable: false),
                    PositivosSemNeumoniaNoConfirmadaCOVID = table.Column<int>(nullable: false),
                    PositivosSospechososNeumoniaNoConfirmadaCOVID = table.Column<int>(nullable: false),
                    SospechososDescartados = table.Column<int>(nullable: false),
                    IdEmpresa = table.Column<int>(nullable: true),
                    EmpresaIdEmpresa = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReporteContagio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReporteContagio_Empresas_EmpresaIdEmpresa",
                        column: x => x.EmpresaIdEmpresa,
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
                    Name = table.Column<string>(nullable: true),
                    IdEmpresa = table.Column<int>(nullable: false),
                    EmpresaIdEmpresa = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unidades_Empresas_EmpresaIdEmpresa",
                        column: x => x.EmpresaIdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trabajadores",
                columns: table => new
                {
                    IdTrabajador = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(maxLength: 50, nullable: false),
                    Genero = table.Column<string>(maxLength: 10, nullable: false),
                    Edad = table.Column<int>(nullable: false),
                    Telefono = table.Column<string>(nullable: false),
                    Correo = table.Column<string>(maxLength: 30, nullable: false),
                    Calle = table.Column<string>(maxLength: 40, nullable: false),
                    NumeroExt = table.Column<string>(maxLength: 5, nullable: false),
                    NumeroInt = table.Column<string>(maxLength: 4, nullable: false),
                    Ciudad = table.Column<string>(maxLength: 40, nullable: false),
                    Estado = table.Column<string>(maxLength: 30, nullable: false),
                    CP = table.Column<string>(maxLength: 6, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(nullable: false),
                    FechaIngreso = table.Column<DateTime>(nullable: false),
                    FechaRegistro = table.Column<DateTime>(nullable: true),
                    NombreUnidad = table.Column<string>(nullable: true),
                    NombreArea = table.Column<string>(nullable: true),
                    IdEmpresa = table.Column<int>(nullable: false),
                    EmpresaIdEmpresa = table.Column<int>(nullable: true),
                    NombreEmpresa = table.Column<string>(nullable: true),
                    IdUnidad = table.Column<int>(nullable: true),
                    UnidadId = table.Column<int>(nullable: true),
                    IdArea = table.Column<int>(nullable: true),
                    AreaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajadores", x => x.IdTrabajador);
                    table.ForeignKey(
                        name: "FK_Trabajadores_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trabajadores_Empresas_EmpresaIdEmpresa",
                        column: x => x.EmpresaIdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trabajadores_Unidades_UnidadId",
                        column: x => x.UnidadId,
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
                    Hipertencion = table.Column<bool>(nullable: false),
                    Asma = table.Column<bool>(nullable: false),
                    SobrePeso = table.Column<bool>(nullable: false),
                    Obesidad = table.Column<bool>(nullable: false),
                    Embarazo = table.Column<bool>(nullable: false),
                    Cancer = table.Column<bool>(nullable: false),
                    Tabaquismo = table.Column<bool>(nullable: false),
                    Alcoholismo = table.Column<bool>(nullable: false),
                    Drogas = table.Column<bool>(nullable: false),
                    NoPersonasCasa = table.Column<int>(nullable: false),
                    TipoCasa = table.Column<string>(maxLength: 20, nullable: false),
                    TipoTransporte = table.Column<string>(maxLength: 20, nullable: false),
                    EspacioTrabajo = table.Column<string>(maxLength: 20, nullable: false),
                    TipoVentilacion = table.Column<string>(maxLength: 20, nullable: false),
                    ContactoLaboral = table.Column<string>(maxLength: 20, nullable: false),
                    TiempoContacto = table.Column<string>(maxLength: 15, nullable: false),
                    FechaHoraRegistro = table.Column<DateTime>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: false),
                    TrabajadorIdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoresRiesgos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactoresRiesgos_Trabajadores_TrabajadorIdTrabajador",
                        column: x => x.TrabajadorIdTrabajador,
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
                    NombreEmpleado = table.Column<string>(nullable: true),
                    FechaHoraInicio = table.Column<DateTime>(nullable: true),
                    FechaHoraFin = table.Column<DateTime>(nullable: true),
                    MotivoIncapacidad = table.Column<string>(nullable: true),
                    TipoIncapacidad = table.Column<string>(nullable: true),
                    SeEncuentraEn = table.Column<string>(nullable: true),
                    IdTrabajador = table.Column<int>(nullable: false),
                    TrabajadorIdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incapacidades", x => x.IdIncapacidad);
                    table.ForeignKey(
                        name: "FK_Incapacidades_Trabajadores_TrabajadorIdTrabajador",
                        column: x => x.TrabajadorIdTrabajador,
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
                    TipoPrueba = table.Column<string>(maxLength: 15, nullable: false),
                    DiagnosticoCovid = table.Column<bool>(nullable: false),
                    PruebaConfirmatoria = table.Column<bool>(nullable: false),
                    SintomasCovid = table.Column<bool>(nullable: false),
                    RadiografiaTorax = table.Column<bool>(nullable: false),
                    Tomografía = table.Column<bool>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: false),
                    TrabajadorIdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pruebas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pruebas_Trabajadores_TrabajadorIdTrabajador",
                        column: x => x.TrabajadorIdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistroPruebas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Temperatura = table.Column<float>(nullable: false),
                    PorcentajeO2 = table.Column<float>(nullable: false),
                    TipoSangre = table.Column<string>(nullable: true),
                    APOlfativa = table.Column<string>(nullable: true),
                    APGustativa = table.Column<string>(nullable: true),
                    Mas15cm = table.Column<int>(nullable: false),
                    Menos15cm = table.Column<int>(nullable: false),
                    PIE3 = table.Column<int>(nullable: false),
                    PIE4 = table.Column<int>(nullable: false),
                    PIE5 = table.Column<int>(nullable: false),
                    Discriminacion = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    Diagnostico = table.Column<string>(nullable: true),
                    ResultadoIgM = table.Column<string>(nullable: true),
                    ResultadoIgG = table.Column<string>(nullable: true),
                    ResultadoPCR = table.Column<string>(nullable: true),
                    IdTrabajador = table.Column<int>(nullable: true),
                    TrabajadorIdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroPruebas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroPruebas_Trabajadores_TrabajadorIdTrabajador",
                        column: x => x.TrabajadorIdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reincorporaciones",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(nullable: false),
                    Aislados15Dias = table.Column<int>(nullable: false),
                    Aislados30Dias = table.Column<int>(nullable: false),
                    ReincorporanSemAnt = table.Column<int>(nullable: false),
                    EmpleadosIncapacidad = table.Column<int>(nullable: false),
                    DiasPerdidosIncapacidadSemAnt = table.Column<int>(nullable: false),
                    DiasAcumuladosMenIncapacidad = table.Column<int>(nullable: false),
                    DiasAcumuladosTot = table.Column<int>(nullable: false),
                    RelTotalTrabajadoresTrabajadoresIncapacidad = table.Column<int>(nullable: false),
                    RelDiasTrabajoDiasIncapacidad = table.Column<int>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: true),
                    TrabajadorIdTrabajador = table.Column<int>(nullable: true),
                    IdEmpresa = table.Column<int>(nullable: true),
                    EmpresaIdEmpresa = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reincorporaciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_Reincorporaciones_Empresas_EmpresaIdEmpresa",
                        column: x => x.EmpresaIdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reincorporaciones_Trabajadores_TrabajadorIdTrabajador",
                        column: x => x.TrabajadorIdTrabajador,
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
                    IdTrabajador = table.Column<int>(nullable: false),
                    TrabajadorIdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiesgoContagios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiesgoContagios_Trabajadores_TrabajadorIdTrabajador",
                        column: x => x.TrabajadorIdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RiesgosTrabajadores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaTrabajo = table.Column<string>(nullable: false),
                    TipoTransporte = table.Column<string>(nullable: false),
                    CantidadPersonas = table.Column<string>(nullable: false),
                    DiagnosticoCovid = table.Column<bool>(nullable: false),
                    ContactoCovid = table.Column<string>(nullable: true),
                    Mas65 = table.Column<bool>(nullable: false),
                    Obesidad = table.Column<bool>(nullable: false),
                    Embarazo = table.Column<bool>(nullable: false),
                    Asma = table.Column<bool>(nullable: false),
                    Fumador = table.Column<bool>(nullable: false),
                    CigarrosDia = table.Column<int>(nullable: false),
                    AniosFumar = table.Column<int>(nullable: false),
                    Diabetes = table.Column<bool>(nullable: false),
                    Hipertension = table.Column<bool>(nullable: false),
                    EnfermedadCronica = table.Column<bool>(nullable: false),
                    NombreECronica = table.Column<string>(nullable: true),
                    Rinitis = table.Column<bool>(nullable: false),
                    Sinusitis = table.Column<bool>(nullable: false),
                    CirugiaNasal = table.Column<bool>(nullable: false),
                    Rinofaringea = table.Column<bool>(nullable: false),
                    NombreRinofaringea = table.Column<bool>(nullable: false),
                    Picante = table.Column<string>(nullable: true),
                    Fiebre = table.Column<bool>(nullable: false),
                    Tos = table.Column<bool>(nullable: false),
                    DolorCabeza = table.Column<bool>(nullable: false),
                    Disnea = table.Column<bool>(nullable: false),
                    Irritabilidad = table.Column<bool>(nullable: false),
                    Diarrea = table.Column<bool>(nullable: false),
                    Escalofrios = table.Column<bool>(nullable: false),
                    Artralgias = table.Column<bool>(nullable: false),
                    Mialgias = table.Column<bool>(nullable: false),
                    Odinofagia = table.Column<bool>(nullable: false),
                    Rinorrea = table.Column<bool>(nullable: false),
                    Polipnea = table.Column<bool>(nullable: false),
                    Vómito = table.Column<bool>(nullable: false),
                    DolorAbdomina = table.Column<bool>(nullable: false),
                    Conjuntivitis = table.Column<bool>(nullable: false),
                    DolorToracico = table.Column<bool>(nullable: false),
                    Anosmia = table.Column<bool>(nullable: false),
                    Disgeusia = table.Column<bool>(nullable: false),
                    Cianosis = table.Column<bool>(nullable: false),
                    Ninguna = table.Column<bool>(nullable: false),
                    TrabajoEnCasa = table.Column<bool>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: true),
                    TrabajadorIdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiesgosTrabajadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiesgosTrabajadores_Trabajadores_TrabajadorIdTrabajador",
                        column: x => x.TrabajadorIdTrabajador,
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
                    EstatusPaciente = table.Column<string>(maxLength: 20, nullable: false),
                    SintomasMayores = table.Column<bool>(nullable: false),
                    SintomasMenores = table.Column<bool>(nullable: false),
                    EstatusEnCasa = table.Column<string>(maxLength: 20, nullable: false),
                    EstatusEnHospital = table.Column<string>(maxLength: 20, nullable: false),
                    FechaSeguimiento = table.Column<DateTime>(nullable: false),
                    IdTrabajador = table.Column<int>(nullable: true),
                    TrabajadorIdTrabajador = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeguimientosCovid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeguimientosCovid_Trabajadores_TrabajadorIdTrabajador",
                        column: x => x.TrabajadorIdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_EmpresaIdEmpresa",
                table: "Areas",
                column: "EmpresaIdEmpresa");

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
                name: "IX_FactoresRiesgos_TrabajadorIdTrabajador",
                table: "FactoresRiesgos",
                column: "TrabajadorIdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_Incapacidades_TrabajadorIdTrabajador",
                table: "Incapacidades",
                column: "TrabajadorIdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_TrabajadorIdTrabajador",
                table: "Pruebas",
                column: "TrabajadorIdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroPruebas_TrabajadorIdTrabajador",
                table: "RegistroPruebas",
                column: "TrabajadorIdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_Reincorporaciones_EmpresaIdEmpresa",
                table: "Reincorporaciones",
                column: "EmpresaIdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Reincorporaciones_TrabajadorIdTrabajador",
                table: "Reincorporaciones",
                column: "TrabajadorIdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_ReporteContagio_EmpresaIdEmpresa",
                table: "ReporteContagio",
                column: "EmpresaIdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_RiesgoContagios_TrabajadorIdTrabajador",
                table: "RiesgoContagios",
                column: "TrabajadorIdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_RiesgosTrabajadores_TrabajadorIdTrabajador",
                table: "RiesgosTrabajadores",
                column: "TrabajadorIdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosCovid_TrabajadorIdTrabajador",
                table: "SeguimientosCovid",
                column: "TrabajadorIdTrabajador");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_AreaId",
                table: "Trabajadores",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_EmpresaIdEmpresa",
                table: "Trabajadores",
                column: "EmpresaIdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_UnidadId",
                table: "Trabajadores",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Unidades_EmpresaIdEmpresa",
                table: "Unidades",
                column: "EmpresaIdEmpresa");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Pruebas");

            migrationBuilder.DropTable(
                name: "RegistroPruebas");

            migrationBuilder.DropTable(
                name: "Reincorporaciones");

            migrationBuilder.DropTable(
                name: "ReporteContagio");

            migrationBuilder.DropTable(
                name: "RiesgoContagios");

            migrationBuilder.DropTable(
                name: "RiesgosTrabajadores");

            migrationBuilder.DropTable(
                name: "SeguimientosCovid");

            migrationBuilder.DropTable(
                name: "UsuariosTrabajadores");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Trabajadores");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Unidades");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
