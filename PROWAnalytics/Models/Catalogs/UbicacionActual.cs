using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace PROWAnalytics.Models
{
    public class UbicacionActual
    {
        [Key]
        public int Identificador { get; set; }

        [Required]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [DisplayName("Fecha de creación")]
        public DateTime FechaCreacion { get; set; }

        public ICollection<Prueba> Pruebas { get; set; }
        public ICollection<PruebaInterna> PruebasInternas { get; set; }
        public ICollection<SeguimientoCovid> SeguimientosCovid { get; set; }
    }
}
