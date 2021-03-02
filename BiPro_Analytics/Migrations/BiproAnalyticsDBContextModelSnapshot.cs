﻿// <auto-generated />
using System;
using BiPro_Analytics.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BiPro_Analytics.Migrations
{
    [DbContext(typeof(BiproAnalyticsDBContext))]
    partial class BiproAnalyticsDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BiPro_Analytics.Models.Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EmpresaIdEmpresa")
                        .HasColumnType("int");

                    b.Property<int>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaIdEmpresa");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Empresa", b =>
                {
                    b.Property<int>("IdEmpresa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Aministrador")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("CP")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("Calle")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<int>("CantEmpleados")
                        .HasColumnType("int");

                    b.Property<string>("Ciudad")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("CodigoEmpresa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DiasLaborales")
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Giro")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<int>("HorasLaborales")
                        .HasColumnType("int");

                    b.Property<decimal>("MaxSueldo")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal>("MinSueldo")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("NumeroExt")
                        .IsRequired()
                        .HasColumnType("nvarchar(5)")
                        .HasMaxLength(5);

                    b.Property<string>("NumeroInt")
                        .HasColumnType("nvarchar(4)")
                        .HasMaxLength(4);

                    b.Property<string>("Puesto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RFC")
                        .IsRequired()
                        .HasColumnType("nvarchar(13)")
                        .HasMaxLength(13);

                    b.Property<string>("RazonSocial")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Seccion")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("SubGiro")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdEmpresa");

                    b.ToTable("Empresas");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.FactorRiesgo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Alcoholismo")
                        .HasColumnType("bit");

                    b.Property<bool>("Asma")
                        .HasColumnType("bit");

                    b.Property<bool>("Cancer")
                        .HasColumnType("bit");

                    b.Property<string>("ContactoLaboral")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<bool>("Diabetes")
                        .HasColumnType("bit");

                    b.Property<bool>("Drogas")
                        .HasColumnType("bit");

                    b.Property<bool>("Embarazo")
                        .HasColumnType("bit");

                    b.Property<bool>("EnfermedadAutoinmune")
                        .HasColumnType("bit");

                    b.Property<bool>("EnfermedadCorazon")
                        .HasColumnType("bit");

                    b.Property<string>("EspacioTrabajo")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime>("FechaHoraRegistro")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Hipertension")
                        .HasColumnType("bit");

                    b.Property<int?>("IdTrabajador")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("NoPersonasCasa")
                        .HasColumnType("int");

                    b.Property<bool>("Obesidad")
                        .HasColumnType("bit");

                    b.Property<bool>("SobrePeso")
                        .HasColumnType("bit");

                    b.Property<bool>("Tabaquismo")
                        .HasColumnType("bit");

                    b.Property<string>("TiempoContacto")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("TipoCasa")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("TipoTransporte")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("TipoVentilacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<int?>("TrabajadorIdTrabajador")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrabajadorIdTrabajador");

                    b.ToTable("FactoresRiesgos");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Incapacidad", b =>
                {
                    b.Property<int>("IdIncapacidad")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("FechaHoraFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaHoraInicio")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<int>("IdTrabajador")
                        .HasColumnType("int");

                    b.Property<string>("MotivoIncapacidad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeEncuentraEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoIncapacidad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TrabajadorIdTrabajador")
                        .HasColumnType("int");

                    b.HasKey("IdIncapacidad");

                    b.HasIndex("TrabajadorIdTrabajador");

                    b.ToTable("Incapacidades");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Prueba", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DiagnosticoCovid")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime>("FechaDiagnostico")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaHoraRegistro")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdTrabajador")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Lugar")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<bool>("RadiografiaTorax")
                        .HasColumnType("bit");

                    b.Property<bool>("SintomasCovid")
                        .HasColumnType("bit");

                    b.Property<string>("TipoPrueba")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<bool>("Tomografia")
                        .HasColumnType("bit");

                    b.Property<int?>("TrabajadorIdTrabajador")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrabajadorIdTrabajador");

                    b.ToTable("Pruebas");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Reincorporaciones", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Aislados15Dias")
                        .HasColumnType("int");

                    b.Property<int>("Aislados30Dias")
                        .HasColumnType("int");

                    b.Property<int>("DiasAcumuladosMenIncapacidad")
                        .HasColumnType("int");

                    b.Property<int>("DiasAcumuladosTot")
                        .HasColumnType("int");

                    b.Property<int>("DiasPerdidosIncapacidadSemAnt")
                        .HasColumnType("int");

                    b.Property<int>("EmpleadosIncapacidad")
                        .HasColumnType("int");

                    b.Property<int?>("EmpresaIdEmpresa")
                        .HasColumnType("int");

                    b.Property<int?>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<int?>("IdTrabajador")
                        .HasColumnType("int");

                    b.Property<int>("ReincorporanSemAnt")
                        .HasColumnType("int");

                    b.Property<int>("RelDiasTrabajoDiasIncapacidad")
                        .HasColumnType("int");

                    b.Property<int>("RelTotalTrabajadoresTrabajadoresIncapacidad")
                        .HasColumnType("int");

                    b.Property<int>("TotalReincorporados")
                        .HasColumnType("int");

                    b.Property<int?>("TrabajadorIdTrabajador")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("EmpresaIdEmpresa");

                    b.HasIndex("TrabajadorIdTrabajador");

                    b.ToTable("Reincorporaciones");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.ReporteContagio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Anio")
                        .HasColumnType("int");

                    b.Property<int?>("EmpresaIdEmpresa")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<int>("PositivosSemAntigeno")
                        .HasColumnType("int");

                    b.Property<int>("PositivosSemLG")
                        .HasColumnType("int");

                    b.Property<int>("PositivosSemNeumoniaNoConfirmadaCOVID")
                        .HasColumnType("int");

                    b.Property<int>("PositivosSemPCR")
                        .HasColumnType("int");

                    b.Property<int>("PositivosSemTAC")
                        .HasColumnType("int");

                    b.Property<int>("PositivosSospechososNeumoniaNoConfirmadaCOVID")
                        .HasColumnType("int");

                    b.Property<int>("Semana")
                        .HasColumnType("int");

                    b.Property<int>("SospechososDescartados")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaIdEmpresa");

                    b.ToTable("ReporteContagio");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.RiesgoContagio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ContactoCovidCasa")
                        .HasColumnType("bit");

                    b.Property<bool>("ContactoCovidFuera")
                        .HasColumnType("bit");

                    b.Property<bool>("ContactoCovidTrabajo")
                        .HasColumnType("bit");

                    b.Property<bool>("Diarrea")
                        .HasColumnType("bit");

                    b.Property<bool>("DificultadRespirar")
                        .HasColumnType("bit");

                    b.Property<bool>("DolorMuscular")
                        .HasColumnType("bit");

                    b.Property<bool>("Escalofrios")
                        .HasColumnType("bit");

                    b.Property<DateTime>("FechaHoraRegistro")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdTrabajador")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<bool>("NauseaVomito")
                        .HasColumnType("bit");

                    b.Property<string>("Olfatometria")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<bool>("Resfriado")
                        .HasColumnType("bit");

                    b.Property<bool>("TempMayor38")
                        .HasColumnType("bit");

                    b.Property<bool>("Tos")
                        .HasColumnType("bit");

                    b.Property<bool>("TosRecurrente")
                        .HasColumnType("bit");

                    b.Property<int?>("TrabajadorIdTrabajador")
                        .HasColumnType("int");

                    b.Property<bool>("ViajesMultitudes")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("TrabajadorIdTrabajador");

                    b.ToTable("RiesgoContagios");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.SeguimientoCovid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EstatusEnCasa")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("EstatusEnHospital")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("EstatusPaciente")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime>("FechaSeguimiento")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdTrabajador")
                        .HasColumnType("int");

                    b.Property<bool>("SintomasMayores")
                        .HasColumnType("bit");

                    b.Property<bool>("SintomasMenores")
                        .HasColumnType("bit");

                    b.Property<int?>("TrabajadorIdTrabajador")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrabajadorIdTrabajador");

                    b.ToTable("SeguimientosCovid");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Trabajador", b =>
                {
                    b.Property<int>("IdTrabajador")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AreaId")
                        .HasColumnType("int");

                    b.Property<string>("CP")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)")
                        .HasMaxLength(6);

                    b.Property<string>("Calle")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("Ciudad")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int?>("Edad")
                        .HasColumnType("int");

                    b.Property<int?>("EmpresaIdEmpresa")
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime?>("FechaIngreso")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Genero")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<int?>("IdArea")
                        .HasColumnType("int");

                    b.Property<int?>("IdEmpresa")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("IdUnidad")
                        .HasColumnType("int");

                    b.Property<string>("Municipio")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("NombreArea")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreEmpresa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreUnidad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroExt")
                        .IsRequired()
                        .HasColumnType("nvarchar(5)")
                        .HasMaxLength(5);

                    b.Property<string>("NumeroInt")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)")
                        .HasMaxLength(4);

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UnidadId")
                        .HasColumnType("int");

                    b.HasKey("IdTrabajador");

                    b.HasIndex("AreaId");

                    b.HasIndex("EmpresaIdEmpresa");

                    b.HasIndex("UnidadId");

                    b.ToTable("Trabajadores");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Unidad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EmpresaIdEmpresa")
                        .HasColumnType("int");

                    b.Property<int>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaIdEmpresa");

                    b.ToTable("Unidades");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.UsuarioEmpresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdEmpresa")
                        .HasColumnType("int");

                    b.Property<Guid>("IdUsuario")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Rol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UsuariosEmpresas");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.UsuarioTrabajador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CodigoEmpresa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrabajadorId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("UsuariosTrabajadores");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Area", b =>
                {
                    b.HasOne("BiPro_Analytics.Models.Empresa", "Empresa")
                        .WithMany("Areas")
                        .HasForeignKey("EmpresaIdEmpresa");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.FactorRiesgo", b =>
                {
                    b.HasOne("BiPro_Analytics.Models.Trabajador", "Trabajador")
                        .WithMany("FactoresRiesgos")
                        .HasForeignKey("TrabajadorIdTrabajador");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Incapacidad", b =>
                {
                    b.HasOne("BiPro_Analytics.Models.Trabajador", "Trabajador")
                        .WithMany("Incapacidades")
                        .HasForeignKey("TrabajadorIdTrabajador");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Prueba", b =>
                {
                    b.HasOne("BiPro_Analytics.Models.Trabajador", "Trabajador")
                        .WithMany("Pruebas")
                        .HasForeignKey("TrabajadorIdTrabajador");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Reincorporaciones", b =>
                {
                    b.HasOne("BiPro_Analytics.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaIdEmpresa");

                    b.HasOne("BiPro_Analytics.Models.Trabajador", "Trabajador")
                        .WithMany()
                        .HasForeignKey("TrabajadorIdTrabajador");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.ReporteContagio", b =>
                {
                    b.HasOne("BiPro_Analytics.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaIdEmpresa");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.RiesgoContagio", b =>
                {
                    b.HasOne("BiPro_Analytics.Models.Trabajador", "Trabajador")
                        .WithMany("RiesgosContagios")
                        .HasForeignKey("TrabajadorIdTrabajador");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.SeguimientoCovid", b =>
                {
                    b.HasOne("BiPro_Analytics.Models.Trabajador", "Trabajador")
                        .WithMany("SeguimientosCovid")
                        .HasForeignKey("TrabajadorIdTrabajador");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Trabajador", b =>
                {
                    b.HasOne("BiPro_Analytics.Models.Area", "Area")
                        .WithMany("Trabajadores")
                        .HasForeignKey("AreaId");

                    b.HasOne("BiPro_Analytics.Models.Empresa", "Empresa")
                        .WithMany("Trabajadores")
                        .HasForeignKey("EmpresaIdEmpresa");

                    b.HasOne("BiPro_Analytics.Models.Unidad", "Unidad")
                        .WithMany("Trabajadores")
                        .HasForeignKey("UnidadId");
                });

            modelBuilder.Entity("BiPro_Analytics.Models.Unidad", b =>
                {
                    b.HasOne("BiPro_Analytics.Models.Empresa", "Empresa")
                        .WithMany("Unidades")
                        .HasForeignKey("EmpresaIdEmpresa");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
