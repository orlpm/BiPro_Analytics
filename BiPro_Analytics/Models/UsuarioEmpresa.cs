using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Models
{
    public class UsuarioEmpresa
    {
        [Key]
        public int Id { get; set; }
        public Guid IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public string Rol { get; set; }
    }
}
