using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace BiPro_Analytics.Models
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
    }
}
