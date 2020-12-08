using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class RegistroPrueba
    {
        public int Id { get; set; }
        public float Temperatura { get; set; }
        public float PorcentajeO2 { get; set; }
        public string TipoSangre { get; set; }
        public string APOlfativa { get; set; }
        public string APGustativa { get; set; }
        public int Mas15cm { get; set; }
        public int Menos15cm { get; set; }
        public int PIE3 { get; set; }
        public int PIE4 { get; set; }
        public int PIE5 { get; set; }
        public int Discriminacion { get; set; }
        public int Total { get; set; }
        public string Diagnostico { get; set; }
        public string ResultadoIgM { get; set; }
        public string ResultadoIgG { get; set; }
        public string ResultadoPCR { get; set; }

        
        [ForeignKey("TrabajadorFK2")]
        public int IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }

    }
}
