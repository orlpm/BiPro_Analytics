using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class FactorRiesgo
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public bool Diabetes { get; set; }

        [Required]
        [DisplayName("Hipertensión")]
        public bool Hipertension { get; set; }

        [Required]
        public bool Asma { get; set; }

        [Required]
        public bool SobrePeso { get; set; }

        [Required]
        public bool Obesidad { get; set; }
        [Required]
        [DisplayName("Enfermedad Autoinmune")]
        public bool EnfermedadAutoinmune { get; set; }
        [Required]
        [DisplayName("Enfermedad del Corazón")]
        public bool EnfermedadCorazon{ get; set; }
        [Required]
        public bool EPOC { get; set; }

        [Required]
        public bool Embarazo { get; set; }

        [Required]
        public bool Cancer { get; set; }

        [Required]
        public bool Tabaquismo { get; set; }

        [Required]
        public bool Alcoholismo { get; set; }

        [Required]
        public bool Drogas { get; set; }

        [Required]
        [DisplayName("No. Personas en vivienda")]
        public int NoPersonasCasa{ get; set; }

        [Required]
        [DisplayName("Tipo de casa")]
        [MaxLength(20)]
        public string TipoCasa{ get; set; }

        [Required]
        [DisplayName("Tipo de transporte")]
        [MaxLength(20)]
        public string TipoTransporte { get; set; }

        [Required]
        [DisplayName("Espacio de trabajo")]
        [MaxLength(20)]
        public string EspacioTrabajo { get; set; }

        [Required]
        [DisplayName("Ventilación trabajo")]
        [MaxLength(40)]
        public string TipoVentilacion { get; set; }

        [Required]
        [DisplayName("Contacto Laboral")]
        [MaxLength(20)]
        public string ContactoLaboral { get; set; }

        [Required]
        [DisplayName("Tiempo de Contacto")]
        [MaxLength(15)]
        public string TiempoContacto  { get; set; }

        [DisplayName("Fecha y Hora de Registro")]
        public DateTime FechaHoraRegistro { get; set; }

        [Required]
        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }
    }
}
