using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Models
{
    public class ArchivosInternos
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; }
        public string Url { get; set; }

        [ForeignKey("PruebaInterna")]
        public int IdPrueba { get; set; }
        public PruebaInterna PruebaInterna { get; set; }
    }
}
