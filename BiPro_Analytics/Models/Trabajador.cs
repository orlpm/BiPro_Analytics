using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BiPro_Analytics.Models
{
    public partial class Trabajador
    {
        [Key]
        public int IdTrabajador { get; set; }

        //[ForeignKey("EmpresasFK")]
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Ciudad { get; set; }
        public string CP { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public ICollection<RiesgosTrabajador> RiesgosTrabajadores { get; set; }
        public ICollection<RegistroPrueba> RegistroPruebas { get; set; }
        public ICollection<Incapacidad> Incapacidades { get; set; }
        //public int IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }

    }
}
