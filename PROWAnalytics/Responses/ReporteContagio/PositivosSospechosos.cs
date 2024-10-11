using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Responses.ReporteContagio
{
    public class PositivosSospechosos
    {
        public int[] CountsPositivos { get; set; } // store label lists
        public int[] CountsNegativos { get; set; } // store label lists
        public int[] CountsSospechosos { get; set; } // store label lists
    }
}
