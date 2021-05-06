using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiPro_Analytics.Models
{
    public partial class Incapacidad
    {
        [Key]
        public int IdIncapacidad { get; set; }

        [Required]
        [DisplayName("Fecha y Hora de Inicio")]
        public DateTime? FechaHoraInicio { get; set; }
        
        [DisplayName("Fecha y Hora de Fin")]
        public DateTime? FechaHoraFin { get; set; }
        
        [Required]
        [DisplayName("Motivo de incapacidad")]
        public string MotivoIncapacidad { get; set; }

        [DisplayName("Tipo de incapacidad")]
        public string TipoIncapacidad { get; set; }

        [DisplayName("Se encuentra en")]
        public string SeEncuentraEn { get; set; }

        [DisplayName("Trabajador")]
        [ForeignKey("Trabajador")]
        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }
    }
}