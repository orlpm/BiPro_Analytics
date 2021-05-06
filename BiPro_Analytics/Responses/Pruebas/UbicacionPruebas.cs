using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Responses.Pruebas
{
    public class UbicacionPruebas
    {
        public int[] CountsPositivos { get; set; } // store label lists
        public int[] CountsNegativos { get; set; } // store label lists
        public int[] CountsSospechosos { get; set; } // store label lists
    }
}
