using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Responses.RiesgosContagios
{
    public class PersonalSintomasCOVID19
    {
        public string Nombre { get; set; }
        public bool TosRecurrente { get; set; }
        public bool Tos { get; set; }
        public bool DificultadRespirar { get; set; }
        public bool TempMayor38 { get; set; }
        public bool Resfriado { get; set; }
        public bool Escalofrios { get; set; }
        public bool DolorMuscular { get; set; }
        public bool NauseaVomito { get; set; }
        public bool Diarrea { get; set; }
    }
}
