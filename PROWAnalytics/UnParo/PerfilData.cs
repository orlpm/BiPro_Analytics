using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PROWAnalytics.Responses;

namespace PROWAnalytics.UnParo
{
    public class PerfilData
    {
        public List<DDLUsuarios> DDLUsuarios { get; set; }
        public List<DDLEmpresa> DDLEmpresas { get; set; }
        public List<DDLUnidad> DDLUnidades { get; set; }
        public List<DDLArea> DDLAreas { get; set; }
        public List<DDLTrabajador> DDLTrabajadores { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdTrabajador { get; set; }
    }
}
