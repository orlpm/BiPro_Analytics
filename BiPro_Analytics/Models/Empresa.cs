using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiPro_Analytics.Models
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

        [MaxLength(50)]
        [Required]
        [DisplayName("Aministrador de tablero")]
        public string Aministrador { get; set; }

        [DisplayName("Puesto")]
        public string Puesto { get; set; }

        [Required]
        [DisplayName("Giro")]
        [MaxLength(40)]
        public string Giro { get; set; }

        [MaxLength(30)]
        [DisplayName("Subgiro")]
        public string SubGiro { get; set; }

        [MaxLength(30)]
        [DisplayName("Sección")]
        public string Seccion { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [MaxLength(40)]
        [DisplayName("Calle")]
        [Required]
        public string Calle { get; set; }

        [MaxLength(5)]
        [DisplayName("Numero Ext")]
        [Required]
        public string NumeroExt { get; set; }

        [MaxLength(4)]
        [DisplayName("Numero Int")]
        public string NumeroInt { get; set; }

        [MaxLength(40)]
        [Required]
        public string Ciudad { get; set; }

        [MaxLength(30)]
        [Required]
        public string Estado { get; set; }

        [MaxLength(6)]
        [Required]
        public string CP { get; set; }

        [DisplayName("No. de Empleados")]
        public int CantEmpleados { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        [DisplayName("Sueldo Mínimo")]
        public decimal MinSueldo { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [DisplayName("Sueldo Máximo")]
        public decimal MaxSueldo { get; set; }

        [DisplayName("Fecha de registro")]
        [DataType(DataType.Date)]
        public DateTime FechaRegistro{ get; set; }

        [DisplayName("Horas Laborales")]
        [Required]
        public int HorasLaborales { get; set; }

        [DisplayName("Dias Laborales")]
        [Required]
        public int DiasLaborales { get; set; }

        [Display(AutoGenerateField = false)]
        public string CodigoEmpresa { get; set; }
        public ICollection<Trabajador> Trabajadores { get; set; }
        public ICollection<Unidad> Unidades { get; set; }
        public ICollection<Area> Areas { get; set; }
    }
}
