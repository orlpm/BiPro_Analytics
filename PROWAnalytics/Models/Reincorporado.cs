using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Models
{
    public class Reincorporado
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Fecha de dianostico")]
        [DataType(DataType.Date)]
        public DateTime FechaDiagnostico { get; set; }
        
        [Required]
        [DisplayName("Fecha de termino de incapacidad")]
        [DataType(DataType.Date)]
        public DateTime FechaTerminoIncapacidad { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de regreso de trabajo")]
        public DateTime FechaRegresoTrabajo { get; set; }
        
        [Required]
        [DisplayName("Dias de incapacidad")]
        public int DiasIncapacidad { get; set; }
        
        [Required]
        [DisplayName("Estudios de secuelas")]
        public bool EtudiosSecuelasPulmonares { get; set; }
        
        [Required]
        [DisplayName("Estudios de secuelas otros")]
        public bool EtudiosSecuelasNoPulmonares { get; set; }

        [Required]
        [DisplayName("Fisicamente capacitado para trabajar")]
        public bool FisicamenteCapacitado{ get; set; }
        
        [Required]
        [DisplayName("Motivado para trabajar")]
        public bool MotivadoTrabajo { get; set; }
        
        [Required]
        [DisplayName("Médico de seguimiento")]
        public bool MedicoSeguimiento { get; set; }


        public DateTime FechaRegistro { get; set; }

        [ForeignKey("Trabajador")]
        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }

    }
}
