using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PROWAnalytics.Models
{
    public partial class Trabajador
    {
        [Key]
        public int IdTrabajador { get; set; }
        
        [DisplayName("Nombre")]
        [MaxLength(50)]
        [Required]
        public string Nombre { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        public int Edad
        {
            get { return (int)((DateTime.Today - FechaNacimiento).Days / 365.25); }
        }

        [DisplayName("Género")]
        [MaxLength(10)]
        [Required]
        public string Genero { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [Required]
        [MaxLength(60)]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [MaxLength(80)]
        [DisplayName("Calle")]
        [Required]
        public string Calle { get; set; }

        [MaxLength(5)]
        [DisplayName("Número Ext")]
        [Required]
        public string NumeroExt { get; set; }

        [MaxLength(8)]
        [DisplayName("Número Int")]
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
        [DisplayName("Fecha de Ingreso")]
        public DateTime? FechaIngreso { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha de Registro")]
        public DateTime? FechaRegistro { get; set; }

        public ICollection<FactorRiesgo> FactoresRiesgos { get; set; }
        public ICollection<RiesgoContagio> RiesgosContagios { get; set; }
        public ICollection<Prueba> Pruebas{ get; set; }
        public ICollection<PruebaInterna> PruebasInternas { get; set; }
        public ICollection<SeguimientoCovid> SeguimientosCovid { get; set; }
        public ICollection<Incapacidad> Incapacidades { get; set; }
        public ICollection<Reincorporado> Reincorporados { get; set; }

        [ForeignKey("Empresa")]
        public int? IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }

        [ForeignKey("Unidad")]
        public int? IdUnidad { get; set; }
        public Unidad Unidad { get; set; }

        [ForeignKey("Area")]
        public int? IdArea { get; set; }
        public Area Area { get; set; }

    }
}