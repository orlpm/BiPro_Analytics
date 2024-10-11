using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Responses
{
    public class ReporteContagioViewModel
    {
        public string NombreEmpresa { get; set; }
        public string NombreArea { get; set; }
        public string Tipo { get; set; }
        public string TipoId { get; set; }
        public int Cantidad { get; set; }
    }
}
