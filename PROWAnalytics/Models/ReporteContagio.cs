using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROWAnalytics.Models
{
    public class ReporteContagio
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Tipo de prueba")]
        public string TipoPrueba { get; set; }

        [Required]
        [RegularExpression("(.*[0-9].*)|(.*[.].*[0-9].*)")]
        [DisplayName("Número de pruebas")]
        public int NumeroPruebas { get; set; }

        [Required]
        [DisplayName("Positivos")]
        public int Positivos { get; set; }

        [Required]
        [DisplayName("Negativos")]
        public int Negativos { get; set; }

        [Required]
        [DisplayName("Número de semana")]
        public int NumeroSemana { get; set; }

        [DisplayName("Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }


        [ForeignKey("Empresa")]
        public int? IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }


        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)")]
        [ForeignKey("Unidad")]
        public int? IdUnidad { get; set; }
        public Unidad Unidad{ get; set; }


        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)")]
        [ForeignKey("Area")]
        public int? IdArea { get; set; }
        public Area Area { get; set; }
    }
}