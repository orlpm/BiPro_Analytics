using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Models
{
    public class Area
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }

        [ForeignKey("Empresa")]
        public int? IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
        public ICollection<Trabajador> Trabajadores { get; set; }
        public ICollection<Prueba> Pruebas { get; set; }
        public ICollection<PruebaInterna> PruebasInternas { get; set; }
    }
}
