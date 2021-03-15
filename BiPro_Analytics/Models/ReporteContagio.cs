using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BiPro_Analytics.Models
{
    public class ReporteContagio
    {
        public int Id { get; set; }

        //public int Semana { get; set; }

        //[DisplayName("Año")]
        //[Required]
        //public int Anio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }

        [Required]
        [DisplayName("Positivos Semanales PCR")]
        public int PositivosSemPCR { get; set; }

        [Required]
        [DisplayName("Positivos Semanales LG")]
        public int PositivosSemLG { get; set; }

        [Required]
        [DisplayName("Positivos Semanales Antigeno")]
        public int PositivosSemAntigeno { get; set; }

        [Required]
        [DisplayName("Positivos Semanales TAC")]
        public int PositivosSemTAC { get; set; }

        [Required]
        [DisplayName("Positivos Semanales Neumonia No confirmada Covid")]
        public int PositivosSemNeumoniaNoConfirmadaCOVID { get; set; }

        [Required]
        [DisplayName("Positivos Sospechosos Neumonia no confirmada Covid")]
        public int PositivosSospechososNeumoniaNoConfirmadaCOVID { get; set; }

        [Required]
        [DisplayName("Sospechosos Descartados")]
        public int SospechososDescartados { get; set; }

        [Required]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)")]
        public int IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }

        [Required]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)")]
        public int IdArea { get; set; }
        public Area Area { get; set; }
    }
}
