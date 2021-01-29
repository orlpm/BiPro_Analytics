using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class UsuarioTrabajador
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int TrabajadorId { get; set; }
        public string CodigoEmpresa { get; set; }
    }
}
