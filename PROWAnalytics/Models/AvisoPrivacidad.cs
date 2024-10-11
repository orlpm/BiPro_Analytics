using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Models
{
    public class AvisoPrivacidad
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public bool Aviso1 { get; set; }
        public bool Aviso2 { get; set; }
    }
}
