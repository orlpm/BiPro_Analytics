using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
        public ICollection<Trabajador> Trabajadores { get; set; }
    }
}
