using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BiPro_Analytics.Models.Catalogs;

namespace BiPro_Analytics.Models
{
    public class SeguimientoCovid
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("SintomaCovid")]
        public int? IdSintoma { get; set; }
        public SintomaCovid SintomaCovid { get; set; }

    [Required]
        [DisplayName("Ubicación")]
        [ForeignKey("UbicacionActual")]
        public int? IdUbicacion { get; set; }
        public UbicacionActual UbicacionActual{ get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Seguimiento")]
        public DateTime FechaSeguimiento { get; set; }

        public DateTime FechaHoraRegistro { get; set; }

        [ForeignKey("Trabajador")]
        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }
    }
}