using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Responses.RiesgosContagios
{
    public class PersonalContactoContagioCOVID19
    {
        public string Nombre { get; set; }
        public bool ContactoCovidCasa { get; set; }
        public bool ContactoCovidTrabajo { get; set; }
        public bool ContactoCovidFuera { get; set; }
        public bool ViajesMultitudes { get; set; }
    }
}
