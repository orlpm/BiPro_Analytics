using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiPro_Analytics.Models
{
    public class Empresa
    {
        [Key]
        public int IdEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string GiroEmpresa { get; set; }
        public string SubGiroEmpresa { get; set; }
        public string Estado { get; set; }
        public string Ciudad { get; set; }
        public string CP { get; set; }
        public int? CantEmpleados { get; set; }
        public decimal? MinSueldo { get; set; }
        public decimal? MaxSueldo { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public int? HorasLaborales { get; set; }
        public int? DiasLaborales { get; set; }
        public ICollection<Trabajador> Trabajadores { get; set; }
    }
}
