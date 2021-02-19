using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class RiesgoContagio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Contacto Covid en Casa")]
        public bool ContactoCovidCasa { get; set; }

        [Required]
        [DisplayName("Contacto Covid en Trabajo")]
        public bool ContactoCovidTrabajo { get; set; }

        [Required]
        [DisplayName("Contacto Covid Exterior")]
        public bool ContactoCovidFuera { get; set; }

        [Required]
        [DisplayName("Viajes o Multitudes")]
        public bool ViajesMultitudes { get; set; }

        [Required]
        [DisplayName("Tos Recurrente")]
        public bool TosRecurrente { get; set; }

        [Required]
        public bool Tos { get; set; }

        [Required]
        [DisplayName("Dificultad Para Respirar")]
        public bool DificultadRespirar { get; set; }

        [Required]
        [DisplayName("Temperatura Mayor a 38°")]
        public bool TempMayor38 { get; set; }

        [Required]
        public bool Resfriado { get; set; }

        [Required]
        public bool Escalofrios { get; set; }

        [Required]
        [DisplayName("Dolor Muscular")]
        public bool DolorMuscular { get; set; }

        [Required]
        [DisplayName("Náuseas o Vómito")]
        public bool NauseaVomito { get; set; }

        [Required]
        public bool Diarrea { get; set; }
        
        [DisplayName("Olfatometría")]
        [MaxLength(20)]
        public string Olfatometria { get; set; }

        [Required]
        [DisplayName("Fecha y hora de registro")]
        public DateTime FechaHoraRegistro { get; set; }


        [Required]
        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }
    }
}
