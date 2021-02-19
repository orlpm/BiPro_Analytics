using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class Reincorporaciones
    {
        public int id { get; set; }
        public int Numero { get; set; }
        public int Aislados15Dias { get; set; }
        public int Aislados30Dias { get; set; }
        public int ReincorporanSemAnt { get; set; }
        public int EmpleadosIncapacidad { get; set; }
        public int DiasPerdidosIncapacidadSemAnt { get; set; }
        public int DiasAcumuladosMenIncapacidad { get; set; }
        public int DiasAcumuladosTot { get; set; }
        public int RelTotalTrabajadoresTrabajadoresIncapacidad { get; set; }
        public int RelDiasTrabajoDiasIncapacidad { get; set; }

        public int? IdTrabajador { get; set; }
        public Trabajador Trabajador { get; set; }

        public int? IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
    }
}
