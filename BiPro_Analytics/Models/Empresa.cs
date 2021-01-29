using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BiPro_Analytics.Models
{
    public class Empresa
    {
        [Key]
        public int IdEmpresa { get; set; }
        [DisplayName("Empresa")]
        [Required]
        public string NombreEmpresa { get; set; }
        [Required]
        [DisplayName("Giro")]
        public string GiroEmpresa { get; set; }
        [DisplayName("Subgiro")]
        public string SubGiroEmpresa { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public string Ciudad { get; set; }
        [Required]
        public string CP { get; set; }
        [DisplayName("No. Empleados")]
        [Required]
        public int? CantEmpleados { get; set; }
        [DisplayName("Sueldo Mínimo")]
        [Required]
        public decimal? MinSueldo { get; set; }
        [DisplayName("Sueldo Máximo")]
        [Required]
        public decimal? MaxSueldo { get; set; }
        [DisplayName("Fecha Ingreso")]
        public DateTime? FechaIngreso { get; set; }
        [DisplayName("Horas Laborales")]
        [Required]
        public int? HorasLaborales { get; set; }
        [DisplayName("Dias Laborales")]
        [Required]
        public int? DiasLaborales { get; set; }
        
        [DisplayName("Codigo de empresa")]
        public string CodigoEmpresa { get; set; }
        public ICollection<Trabajador> Trabajadores { get; set; }
        public ICollection<Unidad> Unidades { get; set; }
        public ICollection<Area> Areas { get; set; }
    }
}
