using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class ReporteContagio
    {
        public int Id { get; set; }

        public int Semana { get; set; }

        [DisplayName("Año")]
        [Required]
        public int Anio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }

        [Required]
        [DisplayName("Positivos Smenales PCR")]
        public int PositivosSemPCR { get; set; }

        [Required]
        [DisplayName("Positivos Smenales LG")]
        public int PositivosSemLG { get; set; }

        [Required]
        [DisplayName("Positivos Smenales Antigeno")]
        public int PositivosSemAntigeno { get; set; }

        [Required]
        [DisplayName("Positivos Smenales TAC")]
        public int PositivosSemTAC { get; set; }

        [Required]
        [DisplayName("Positivos Smenales Neumonia No confirmada Covid")]
        public int PositivosSemNeumoniaNoConfirmadaCOVID { get; set; }

        [Required]
        [DisplayName("Positivos Sospechosos Neumonia no confirmada Covid")]
        public int PositivosSospechososNeumoniaNoConfirmadaCOVID { get; set; }

        [Required]
        [DisplayName("Sospechosos Descartados")]
        public int SospechososDescartados { get; set; }

        public int? IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
    }
}
