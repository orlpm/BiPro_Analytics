using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class Finanzas
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Ubicación")]
        public string EstatusReincorporado { get; set; }

        public DateTime FechaRegistro { get; set; }

        
        [ForeignKey("Trabajador")]
        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }

    }
}