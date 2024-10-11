using System;

namespace BiPro_Analytics.Responses
{
    public class SeguimientoCovidViewModel
    {
        public string NombreTrabajador { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string NombreUnidad { get; set; }
        public string NombreArea { get; set; }
        public string EstatusPaciente { get; set; }
        public string SintomasMayores { get; set; }
        public string SintomasMenores { get; set; }
        public string EstatusEnCasa { get; set; }
        public string EstatusEnHospital { get; set; }
        public DateTime FechaSeguimiento { get; set; }
        public string YaRealizoPrueba { get; set; }
        public string YaRealizoSeguimientoCovid { get; set; }
    }
}
