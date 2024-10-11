using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Models.Catalogs
{
    public class DiagnosticoCovid
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Diagnostico { get; set; }
        public ICollection<Prueba> Pruebas { get; set; }
    }
}
