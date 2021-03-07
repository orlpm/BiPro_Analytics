using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Responses
{
    public class CondicionesConstantesRiesgoContagio
    {
        public string Nombre { get; set; }
        public int NoPersonasCasa { get; set; }
        public string TipoCasa { get; set; }
        public string TipoTransporte { get; set; }
        public string EspacioTrabajo { get; set; }
        public string TipoVentilacion { get; set; }
        public string ContactoLaboral { get; set; }
        public string TiempoContacto { get; set; }

    }
}
