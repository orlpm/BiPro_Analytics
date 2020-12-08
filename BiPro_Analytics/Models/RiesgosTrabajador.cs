using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class RiesgosTrabajador
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Area de trabajo")]
        public string AreaTrabajo { get; set; }
        [Required]
        [DisplayName("Transporte usual")]
        public string TipoTransporte { get; set; }
        [Required]
        [DisplayName("Personas vivienda")]
        public string CantidadPersonas { get; set; }
        public bool DiagnosticoCovid { get; set; }
        public string ContactoCovid { get; set; }
        public bool Mas65 { get; set; }
        public bool Obesidad { get; set; }
        public bool Embarazo { get; set; }
        public bool Asma { get; set; }
        public bool Fumador { get; set; }
        public int CigarrosDia { get; set; }
        public int AniosFumar { get; set; }
        public bool Diabetes { get; set; }
        public bool Hipertension { get; set; }
        public bool EnfermedadCronica { get; set; }
        public string NombreECronica { get; set; }
        public bool Rinitis { get; set; }
        public bool Sinusitis { get; set; }
        public bool CirugiaNasal { get; set; }
        public bool Rinofaringea { get; set; }
        public bool NombreRinofaringea { get; set; }
        public string Picante { get; set; }
        public bool Fiebre { get; set; }
        public bool Tos { get; set; }
        public bool DolorCabeza { get; set; }
        public bool Disnea { get; set; }
        public bool Irritabilidad { get; set; }
        public bool Diarrea { get; set; }
        public bool Escalofrios { get; set; }
        public bool Artralgias { get; set; }
        public bool Mialgias { get; set; }
        public bool Odinofagia { get; set; }
        public bool Rinorrea { get; set; }
        public bool Polipnea { get; set; }
        public bool Vómito { get; set; }
        public bool DolorAbdomina { get; set; }
        public bool Conjuntivitis { get; set; }
        public bool DolorToracico { get; set; }
        public bool Anosmia { get; set; }
        public bool Disgeusia { get; set; }
        public bool Cianosis { get; set; }
        public bool Ninguna { get; set; }
        public bool TrabajoEnCasa { get; set; }
        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }

    }
}
