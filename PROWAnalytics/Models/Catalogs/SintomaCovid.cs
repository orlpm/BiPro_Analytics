using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Models.Catalogs
{
    public class SintomaCovid
    {
        [Key]
        public int IdSintoma { get; set; }
        public string Sintoma { get; set;}
        public IEnumerable<SeguimientoCovid> SeguimientosCovid { get; set; }
    }
}
