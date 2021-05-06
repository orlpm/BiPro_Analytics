using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class Productividad
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Caida en la productividad")]
        public string CaidaProductividad { get; set; }

        [Required]
        [DisplayName("Incremento de horas de trabajo")]
        public string IncrementoHorasTrabajo { get; set; }

        [Required]
        [DisplayName("Días de retraso en la entrega")]
        public int DiasRetrasoEntrega { get; set; }

        [Required]
        [DisplayName("Trabajo absorbido de otros compañeros")]
        public bool TrabajoAbsobido { get; set; }

        [ForeignKey("Trabajador")]
        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }
    }
}
