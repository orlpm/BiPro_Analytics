using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Responses
{
    public class PiramidePoblacional
    {
        public string[] Labels { get; set; } // store count lists 
        public int[] CountsMujeres { get; set; } // store label lists
        public int[] CountsHombres { get; set; } // store label lists
    }
}
