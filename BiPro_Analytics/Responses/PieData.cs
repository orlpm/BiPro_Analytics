﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Responses
{
    public class PieData
    {
        public string [] Labels { get; set; } // store count lists 
        public int [] Counts { get; set; } // store label lists
    }
}
