using PROWAnalytics.Models.Catalogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Models
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

        [DisplayName("Tipo de Prueba")]
        [MaxLength(50)]
        public string TipoPrueba { get; set; }

        [Required]
        [DisplayName("Diagnostico Covid")]
        public string DiagnosticoCovid { get; set; }

        public DateTime FechaHoraRegistro { get; set; }


        [ForeignKey("Trabajador")]
        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }

        
        [ForeignKey("UbicacionActual")]
        public int? UbicacionId { get; set; }
        public UbicacionActual UbicacionActual { get; set; }

        [ForeignKey("Unidad")]
        public int? IdUnidad { get; set; }
        public Unidad Unidad { get; set; }

        [ForeignKey("Area")]
        public int? IdArea { get; set; }
        public Area Area { get; set; }
    }
}
