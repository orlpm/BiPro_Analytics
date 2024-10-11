using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROWAnalytics.Models
{
    public class Empresa
    {
        [Key]
        public int IdEmpresa { get; set; }

        [DisplayName("Empresa")]
        [MaxLength(50)]
        [Required]
        public string Nombre { get; set; }

        [DisplayName("Razón Social")]
        [MaxLength(50)]
        [Required]
        public string RazonSocial { get; set; }

        [MaxLength(13)]
        [Required]
        public string RFC { get; set; }

        [MaxLength(30)]
        [Required]
        [DisplayName("Actividad Principal")]
        public string ActividadPrincipal { get; set; }

        [Required]
        [DisplayName("Sector")]
        [MaxLength(40)]
        public string Sector { get; set; }
        [MaxLength(40)]
        [DisplayName("Calle")]
        [Required]
        public string Calle { get; set; }

        [MaxLength(5)]
        [DisplayName("Número Ext")]
        [Required]
        public string NumeroExt { get; set; }

        [MaxLength(4)]
        [DisplayName("Número Int")]
        public string NumeroInt { get; set; }

        [MaxLength(100)]
        public string Colonia { get; set; }

        [MaxLength(50)]
        [DisplayName("Ciudad")]
        public string Ciudad { get; set; }

        [MaxLength(50)]
        [DisplayName("Municipio o delegación")]
        public string Municipio { get; set; }

        [MaxLength(30)]
        [Required]
        public string Estado { get; set; }

        [MaxLength(6)]
        [Required]
        public string CP { get; set; }

        [MaxLength(50)]
        [Required]
        [DisplayName("Aministrador de tablero")]
        public string Aministrador { get; set; }

        [DisplayName("Puesto")]
        public string Puesto { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [Required]
        [MaxLength(60)]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [DisplayName("No. de Empleados")]
        public int CantidadEmpleados { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        [DisplayName("Sueldo Mínimo")]
        public decimal SueldoMinimo { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [DisplayName("Sueldo Máximo")]
        public decimal SueldoMaximo { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [DisplayName("Sueldo Promedio")]
        public decimal SueldoPromedio { get; set; }
        
        [DisplayName("Número de sucursales")]
        public int NumeroSucursales { get; set; }

        public bool Comedor { get; set; }
        
        [DisplayName("Transporte para trabajadores")]
        public bool TransporteTrabajadores { get; set; }

        [DisplayName("Servicio médico")]
        public bool ServicioMedico { get; set; }

        [DisplayName("Seguro de gastos médicos mayores")]
        public bool SGMM { get; set; }

        [DisplayName("Trabajadores con SGMM")]
        public int TrabajadoresConSGMM { get; set; }

        [DisplayName("Nombre de Aseguradora")]
        public string NombreAseguradora { get; set; }

        [DisplayName("Nombre de agente de seguros")]
        public string NombreAgenteSeguros{ get; set; }

        [Range(0, 24, ErrorMessage = "Ingresar no mas de 24 hrs")]
        [DisplayName("Horas Laborales")]
        [Required]
        public int HorasLaborales { get; set; }

        [DisplayName("Días Laborales")]
        [Required]
        public int DiasLaborales { get; set; }

        [DisplayName("Fecha de registro")]
        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }

        [Display(AutoGenerateField = false)]

        [DisplayName("Código empresa")]
        public string CodigoEmpresa { get; set; }
        public bool DescartarUnidades { get; set; }
        public bool DescartarAreas { get; set; }

        public ICollection<Trabajador> Trabajadores { get; set; }
        public ICollection<Unidad> Unidades { get; set; }
        public ICollection<Area> Areas { get; set; }
    }
}
