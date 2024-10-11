using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Responses.RiesgosContagios
{
    public class PersonalSeguimiento
    {
        public string Nombre { get; set; }
        public bool Cuestionario { get; set; }
        public bool Prueba { get; set; }
        public bool Olfatometria { get; set; }
        public bool Ninguno { get; set; }
    }
}
