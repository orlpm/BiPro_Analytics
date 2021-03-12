using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class SeguimientoCovid
    {
        public int Id { get; set; }

        [MaxLength(20)]
        [DisplayName("Estatus de Paciente")]
        [Required]
        public string EstatusPaciente { get; set; }

        [DisplayName("Sintomas Mayores")]
        [Required]
        public bool SintomasMayores { get; set; }

        [DisplayName("Sintomas Menores")]
        [Required]
        public bool SintomasMenores { get; set; }

        [Required]
        [DisplayName("Estatus en casa")]
        public int EstatusEnCasa { get; set; }

        [Required]
        [DisplayName("Estatus en Hospital")]
        public int EstatusEnHospital { get; set; }

        //[MaxLength(20)]
        //[DisplayName("Estatus en casa")]
        //[Required]
        //public string EstatusEnCasa { get; set; }

        //[MaxLength(20)]
        //[DisplayName("Estatus en Hospital")]
        //[Required]
        //public string EstatusEnHospital { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Seguimiento")]
        public DateTime FechaSeguimiento { get; set; }


        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }
    }
}
