using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiPro_Analytics.Models
{
    public partial class Incapacidad
    {
        [Key]
        public int IdIncapacidad { get; set; }
        public string NombreEmpleado { get; set; }
        public DateTime? FechaHoraInicio { get; set; }
        public DateTime? FechaHoraFin { get; set; }
        public string MotivoIncapacidad { get; set; }
        public string TipoIncapacidad { get; set; }
        public string SeEncuentraEn { get; set; }


        [ForeignKey("TrabajadorFK3")]
        public int IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }
    }
}
