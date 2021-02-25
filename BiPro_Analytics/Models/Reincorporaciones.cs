using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class Reincorporaciones
    {
        [Key]
        public int id { get; set; }

        [Required]
        [DisplayName("Total de reincorporados")]
        public int TotalReincorporados { get; set; }

        [Required]
        [DisplayName("No. Aislados 15 días")]
        public int Aislados15Dias { get; set; }

        [Required]
        [DisplayName("No. Aislados 30 días")]
        public int Aislados30Dias { get; set; }

        [DisplayName("No. Reicorporaciones Semana Anterior")]
        public int ReincorporanSemAnt { get; set; }

        [DisplayName("No. Empleados en Incapacidad ")]
        public int EmpleadosIncapacidad { get; set; }

        [DisplayName("Días perdiddos Incapacidad Semana Anterior")]
        public int DiasPerdidosIncapacidadSemAnt { get; set; }

        [DisplayName("Día acumuladoas mensuales de incapacidad")]
        public int DiasAcumuladosMenIncapacidad { get; set; }

        [DisplayName("Días Acumulados")]
        public int DiasAcumuladosTot { get; set; }

        [DisplayName("Relación de trabajadores acrivos y en incapacidad")]
        public int RelTotalTrabajadoresTrabajadoresIncapacidad { get; set; }

        [DisplayName("Relación de días de trabajo y días de incapacidad")]
        public int RelDiasTrabajoDiasIncapacidad { get; set; }

        [DisplayName("Trabajador")]
        [ForeignKey("TrabajadorFK")]
        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }

        [DisplayName("Empresa")]
        [ForeignKey("EmpresaFK")]
        public int? IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
    }
}
