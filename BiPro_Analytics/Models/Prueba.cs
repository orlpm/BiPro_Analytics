using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class Prueba
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Fecha y Hora Diagnostico")]
        [DataType(DataType.DateTime)]
        public DateTime FechaDiagnostico { get; set; }

        [Required]
        [MaxLength(40)]
        public string Lugar { get; set; }

        [Required]
        [DisplayName("Tipo de Prueba")]
        [MaxLength(30)]
        public string TipoPrueba { get; set; }

        [Required]
        [DisplayName("Diagnostico Covid")]
        [MaxLength(20)]
        public string DiagnosticoCovid { get; set; }

        [Required]
        [DisplayName("Sintomas Covid")]
        public bool SintomasCovid { get; set; }

        [Required]
        [DisplayName("Radiografía de Torax")]
        public bool RadiografiaTorax { get; set; }

        [Required]
        [DisplayName("Tomografía")]
        public bool Tomografia { get; set; }

        public DateTime FechaHoraRegistro { get; set; }

        [Required]
        public int? IdTrabajador { get; set; }

        public Trabajador Trabajador { get; set; }

        [Required]
        public int UbicacionId { get; set; }
    }
}
