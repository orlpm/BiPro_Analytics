using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Models
{
    public class Archivos
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; }
        public string Url { get; set; }

        [ForeignKey("IdPruevaFK")]
        public int IdPrueba { get; set; }
        public Prueba Prueba { get; set; }
    }
}
