using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BiPro_Analytics.Models
{
    public partial class Trabajador
    {
        [Key]
        public int IdTrabajador { get; set; }
        
        [DisplayName("Nombre")]
        [MaxLength(50)]
        [Required]
        public string Nombre { get; set; }

        [DisplayName("Genero")]
        [MaxLength(10)]
        [Required]
        public string Genero { get; set; }

        [Display(AutoGenerateField = false)]
        public int? Edad { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Required]
        [MaxLength(30)]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [MaxLength(40)]
        [DisplayName("Calle")]
        [Required]
        public string Calle { get; set; }

        [MaxLength(5)]
        [DisplayName("Numero Ext")]
        [Required]
        public string NumeroExt { get; set; }

        [MaxLength(4)]
        [DisplayName("Numero Int")]
        [Required]
        public string NumeroInt { get; set; }
        
        [MaxLength(6)]
        [Required]
        public string CP { get; set; }

        [MaxLength(30)]
        [Required]
        public string Estado { get; set; }

        [MaxLength(50)]
        [Required]
        public string Municipio { get; set; }

        [MaxLength(40)]
        [Required]
        public string Ciudad { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Ingreso")]
        public DateTime? FechaIngreso { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha de Registro")]
        public DateTime? FechaRegistro { get; set; }

        [DisplayName("Unidad")]
        public string NombreUnidad { get; set; }

        [DisplayName("Area")]
        public string NombreArea { get; set; }

        public ICollection<FactorRiesgo> FactoresRiesgos { get; set; }
        public ICollection<RiesgoContagio> RiesgosContagios { get; set; }
        public ICollection<Prueba> Pruebas{ get; set; }
        public ICollection<SeguimientoCovid> SeguimientosCovid { get; set; }
        public ICollection<Incapacidad> Incapacidades { get; set; }

        [Required]
        public int? IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }

        //[Display(AutoGenerateField = false)]
        [DisplayName("Area")]
        public string NombreEmpresa { get; set; }

        public int? IdUnidad { get; set; }
        public Unidad Unidad { get; set; }

        public int? IdArea { get; set; }
        public Area Area { get; set; }

    }
}