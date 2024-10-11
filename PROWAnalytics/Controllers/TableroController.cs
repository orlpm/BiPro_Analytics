using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PROWAnalytics.Data;
using PROWAnalytics.Models;
using PROWAnalytics.Responses;
using PROWAnalytics.UnParo;
using PROWAnalytics.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PROWAnalytics.Controllers
{
    [Authorize(Roles = "Admin, AdminEmpresa")]
    public class TableroController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;
        private readonly ClaimsPrincipal _currentUser;

        public TableroController(BiproAnalyticsDBContext context)
        {
            _context = context;
            _currentUser = this.User;
        }
        // GET: TableroController

        public async Task<IActionResult> Dashboard()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            ViewBag.Unidades = perfilData.DDLUnidades;
            ViewBag.Areas = perfilData.DDLAreas;
            ViewBag.Empresas = perfilData.DDLEmpresas;
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            List<Area> areas = await _context.Areas
                .ToListAsync();

            //if (idUnidad != null)
            //{
            //    List<Unidad> unidades = await _context.Unidades
            //    .Where(u => u == idArea).ToListAsync();
            //}

            return View();
        }
        public IActionResult Charts(int? IdEmpresa)
        {
            ViewBag.IdEmpresa = IdEmpresa;

            return View();
        }

        public async Task<int> GetIdEmpresa()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            return perfilData.IdEmpresa.GetValueOrDefault();
        }

        public JsonResult PiramidePoblacional(int? idEmpresa)
        {
            List<PiramidePoblacional> piramidePoblacionallist = new List<PiramidePoblacional>();
            List<Trabajador> edadesMujeres;
            List<Trabajador> edadesHombres;

            if (idEmpresa == null)
            {
                edadesMujeres = _context.Trabajadores
                .Where(r => r.Genero == "Femenino").ToList();

                edadesHombres = _context.Trabajadores
                    .Where(r => r.Genero == "Masculino").ToList();
            }
            else
            {
                edadesMujeres = _context.Trabajadores
                .Where(r => r.Genero == "Femenino" && r.IdEmpresa == idEmpresa).ToList();

                edadesHombres = _context.Trabajadores
                    .Where(r => r.Genero == "Masculino" && r.IdEmpresa == idEmpresa).ToList();
            }

            string[] lbls = new string[] { " Covid" };
            int[] cntsMujeres = new int[]
            {
                edadesMujeres.Where(r=>r.Edad > 18 && r.Edad <= 25).ToList().Count(),
                edadesMujeres.Where(r=>r.Edad > 25 && r.Edad <= 35).ToList().Count(),
                edadesMujeres.Where(r=>r.Edad > 35 && r.Edad <= 45).ToList().Count(),
                edadesMujeres.Where(r=>r.Edad > 45 && r.Edad <= 55).ToList().Count(),
                edadesMujeres.Where(r=>r.Edad > 55 && r.Edad <= 65).ToList().Count(),
                edadesMujeres.Where(r=>r.Edad > 65 && r.Edad <= 75).ToList().Count(),
                edadesMujeres.Where(r=>r.Edad > 75 && r.Edad <= 85).ToList().Count(),
                edadesMujeres.Where(r=>r.Edad > 85).ToList().Count()
            };
            int[] cntsHombres = new int[]
            {
                edadesHombres.Where(r=>r.Edad > 18 && r.Edad <= 25).ToList().Count() * -1,
                edadesHombres.Where(r=>r.Edad > 25 && r.Edad <= 35).ToList().Count() * -1,
                edadesHombres.Where(r=>r.Edad > 35 && r.Edad <= 45).ToList().Count() * -1,
                edadesHombres.Where(r=>r.Edad > 45 && r.Edad <= 55).ToList().Count() * -1,
                edadesHombres.Where(r=>r.Edad > 55 && r.Edad <= 65).ToList().Count() * -1,
                edadesHombres.Where(r=>r.Edad > 65 && r.Edad <= 75).ToList().Count() * -1,
                edadesHombres.Where(r=>r.Edad > 75 && r.Edad <= 85).ToList().Count() * -1,
                edadesHombres.Where(r=>r.Edad > 85 ).ToList().Count() * -1
            };

            PiramidePoblacional piramidePoblacional = new PiramidePoblacional();
            piramidePoblacional.Labels = lbls;
            piramidePoblacional.CountsMujeres = cntsMujeres;
            piramidePoblacional.CountsHombres = cntsHombres;

            piramidePoblacionallist.Add(piramidePoblacional);

            return Json(piramidePoblacional);
        }

        public JsonResult CondicionesRiesgo(int? idEmpresa)
        {
            List<CondicionesRiesgo> condicionesRiesgos = new List<CondicionesRiesgo>();
            List<FactorRiesgo> factorRiesgosM;
            List<FactorRiesgo> factorRiesgosH;

            if (idEmpresa == null)
            {
                factorRiesgosM = _context.Trabajadores.Where(t => t.Genero == "Femenino")
                    .SelectMany(f => f.FactoresRiesgos).ToList();
                factorRiesgosH = _context.Trabajadores.Where(t => t.Genero == "Masculino")
                    .SelectMany(f => f.FactoresRiesgos).ToList();
            }
            else
            {
                factorRiesgosM = _context.Trabajadores.Where(t => t.Genero == "Femenino" && t.IdEmpresa == idEmpresa)
                    .SelectMany(f => f.FactoresRiesgos).ToList();
                factorRiesgosH = _context.Trabajadores.Where(t => t.Genero == "Masculino" && t.IdEmpresa == idEmpresa)
                    .SelectMany(f => f.FactoresRiesgos).ToList();
            }

            var diabetesM = factorRiesgosM.Where(x => x.Diabetes).ToList().Count();
            var diabetesH = factorRiesgosH.Where(x => x.Diabetes).ToList().Count();

            var hipertensionM = factorRiesgosM.Where(x => x.Hipertension).ToList().Count();
            var hipertensionH = factorRiesgosH.Where(x => x.Hipertension).ToList().Count();

            var asmaM = factorRiesgosM.Where(x => x.Asma).ToList().Count();
            var asmaH = factorRiesgosH.Where(x => x.Asma).ToList().Count();

            var sobrePesoM = factorRiesgosM.Where(x => x.SobrePeso).ToList().Count();
            var sobrePesoH = factorRiesgosH.Where(x => x.SobrePeso).ToList().Count();

            var ObesidadM = factorRiesgosM.Where(x => x.Obesidad).ToList().Count();
            var ObesidadH = factorRiesgosH.Where(x => x.Obesidad).ToList().Count();

            var EnfermedadAutoinmuneM = factorRiesgosM.Where(x => x.EnfermedadAutoinmune).ToList().Count();
            var EnfermedadAutoinmuneH = factorRiesgosH.Where(x => x.EnfermedadAutoinmune).ToList().Count();

            var EnfermedadCorazonM = factorRiesgosM.Where(x => x.EnfermedadCorazon).ToList().Count();
            var EnfermedadCorazonH = factorRiesgosH.Where(x => x.EnfermedadCorazon).ToList().Count();

            var CancerM = factorRiesgosM.Where(x => x.Cancer).ToList().Count();
            var CancerH = factorRiesgosH.Where(x => x.Cancer).ToList().Count();

            var EPOCM = factorRiesgosM.Where(x => x.EPOC).ToList().Count();
            var EPOCH = factorRiesgosH.Where(x => x.EPOC).ToList().Count();

            var TabaquismoM = factorRiesgosM.Where(x => x.Tabaquismo).ToList().Count();
            var TabaquismoH = factorRiesgosH.Where(x => x.Tabaquismo).ToList().Count();

            var AlcoholismoM = factorRiesgosM.Where(x => x.ConsumoAlcohol).ToList().Count();
            var AlcoholismoH = factorRiesgosH.Where(x => x.ConsumoAlcohol).ToList().Count();

            var DrogasM = factorRiesgosM.Where(x => x.FarmacosDrogas).ToList().Count();
            var DrogasH = factorRiesgosH.Where(x => x.FarmacosDrogas).ToList().Count();

            var EmbarazoM = factorRiesgosM.Where(x => x.Embarazo).ToList().Count();

            int[] cntsMujeres = new int[]
                {
                    diabetesM,
                    hipertensionM,
                    sobrePesoM,
                    ObesidadM,
                    EnfermedadAutoinmuneM,
                    EnfermedadCorazonM,
                    CancerM,
                    EPOCM,
                    TabaquismoM,
                    AlcoholismoM,
                    DrogasM,
                    EmbarazoM

                };

            int[] cntsHombres = new int[]
                {
                    diabetesH,
                    hipertensionH,
                    sobrePesoH,
                    ObesidadH,
                    EnfermedadAutoinmuneH,
                    EnfermedadCorazonH,
                    CancerH,
                    EPOCH,
                    TabaquismoH,
                    AlcoholismoH,
                    DrogasH,
                    0
                };

            CondicionesRiesgo condicionesRiesgo = new CondicionesRiesgo();
            condicionesRiesgo.CountsMujeres = cntsMujeres;
            condicionesRiesgo.CountsHombres = cntsHombres;

            return Json(condicionesRiesgo);
        }

        public JsonResult RiesgosExpocisionCasaTransporte(int? idEmpresa)
        {
            List<PieData> pieDatas = new List<PieData>();
            PieData pieData = new PieData();
            int mayor3;
            int multiplesFamilias;
            int transportePublico;

            if (idEmpresa == null)
            {
                mayor3 = _context.FactoresRiesgos.Where(f => f.NoPersonasCasa > 3).Count();
                multiplesFamilias = _context.FactoresRiesgos
                    .Where(f => f.TipoCasa != "1" ).Count();
                transportePublico = _context.FactoresRiesgos
                    .Where(f => f.TipoTransporte == "Autobús" || f.TipoTransporte == "Metro" || f.TipoTransporte == "Autobus de Empresa" || f.TipoTransporte == "Combi").Count();
            }
            else
            {
                mayor3 = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(t => t.FactoresRiesgos).Where(f => f.NoPersonasCasa > 3).Count();
                multiplesFamilias = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(t => t.FactoresRiesgos)
                    .Where(f => f.TipoCasa != "1").Count();
                transportePublico = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(t => t.FactoresRiesgos)
                    .Where(f => f.TipoTransporte == "Autobús" || f.TipoTransporte == "Metro" || f.TipoTransporte == "Autobus de Empresa" || f.TipoTransporte == "Combi").Count();
            }

            pieData.Counts = new int[]
            {
                mayor3,
                multiplesFamilias,
                transportePublico
            };

            return Json(pieData);
        }

        public JsonResult RiesgosEspacioLaboral(int? idEmpresa)
        {
            List<PieData> pieDatas = new List<PieData>();
            PieData pieData = new PieData();
            int espacioTrabajo;
            int tipoVentilacion;

            if (idEmpresa == null)
            {
                espacioTrabajo = _context.FactoresRiesgos.Where(f => f.EspacioTrabajo == "Cuarto" || f.EspacioTrabajo == "Salón amplio").Count();
                tipoVentilacion = _context.FactoresRiesgos.Where(f => f.TipoVentilacion == "Sin ventilacion").Count();
            }
            else
            {
                espacioTrabajo = _context.Trabajadores
                    .Where(t => t.IdEmpresa == idEmpresa).SelectMany(t => t.FactoresRiesgos).Where(f => f.EspacioTrabajo == "Cuarto" || f.EspacioTrabajo == "Salón amplio").Count();
                tipoVentilacion = _context.Trabajadores
                    .Where(t => t.IdEmpresa == idEmpresa).SelectMany(t => t.FactoresRiesgos).Where(f => f.TipoVentilacion == "Sin ventilacion").Count();
            }

            pieData.Counts = new int[]
            {
                espacioTrabajo,
                tipoVentilacion
            };

            return Json(pieData);
        }

        [HttpGet]
        public JsonResult EmpleadosContactoCovid(int? idEmpresa)
        {
            List<PieData> pieDatas = new List<PieData>();
            List<RiesgoContagio> riesgoContagios;

            if (idEmpresa == null)
                riesgoContagios = _context.RiesgoContagios.Where(r => r.ContactoCovidCasa || r.ContactoCovidFuera || r.ContactoCovidTrabajo).ToList();
            else
                riesgoContagios = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                .SelectMany(t => t.RiesgosContagios).Where(r => r.ContactoCovidCasa || r.ContactoCovidFuera || r.ContactoCovidTrabajo).ToList();

            var total = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                .SelectMany(t => t.RiesgosContagios).Count();

            var otros = total - riesgoContagios.Count();

            string[] lbls = new string[] { "Contacto Covid", "Otros" };
            int[] cnts = new int[]
            {
                riesgoContagios.Count,
                otros
            };

            PieData pieData = new PieData();
            pieData.Labels = lbls;
            pieData.Counts = cnts;

            pieDatas.Add(pieData);

            return Json(pieData);
        }

        public JsonResult SintomasCOVID(int? idEmpresa)
        {
            List<RiesgoContagio> sintomasCovid;
            List<PieData> pieDatas = new List<PieData>();
            PieData pieData = new PieData();


            if (idEmpresa == null)
                sintomasCovid = _context.RiesgoContagios
                    .Where(r => r.Diarrea || r.DificultadRespirar || r.DolorMuscular || r.Escalofrios
                                    || r.NauseaVomito || r.Resfriado || r.TempMayor38 || r.Tos || r.TosRecurrente).ToList();
            else
                sintomasCovid = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(t => t.RiesgosContagios)
                    .Where(r => r.Diarrea || r.DificultadRespirar || r.DolorMuscular || r.Escalofrios
                                    || r.NauseaVomito || r.Resfriado || r.TempMayor38 || r.Tos || r.TosRecurrente).ToList();


            var otros = _context.RiesgoContagios.Count() - sintomasCovid.Count();

            pieData.Counts = new int[]
            {
                sintomasCovid.Count(),
                otros
            };

            return Json(pieData);
        }

        public JsonResult EmpleadosAnosmiaHiposmia(int? idEmpresa)
        {
            List<PieData> pieDatas = new List<PieData>();
            PieData pieData = new PieData();
            int anosmia;

            if (idEmpresa == null)
                anosmia = _context.RiesgoContagios.Where(r => r.Olfatometria == "Anosmia").ToList().Count();
            else
                anosmia = _context.Trabajadores
                    .Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(t => t.RiesgosContagios).Where(r => r.Olfatometria == "Anosmia").ToList().Count();

            var hiposmia = _context.Trabajadores
                .Where(t => t.IdEmpresa == idEmpresa)
                .SelectMany(t => t.RiesgosContagios).Where(r => r.Olfatometria == "Hiposmia").ToList().Count();

            var normal = _context.RiesgoContagios.ToList().Count() - anosmia - hiposmia;

            string[] lbls = new string[] { "Anosmia", "Hiposmia", "Normal" };

            pieData.Labels = lbls;

            pieData.Counts = new int[]
            {
                anosmia,
                hiposmia,
                normal
            };

            return Json(pieData);

        }

        public int CalculaEdad(DateTime fechaNacimiento)
        {
            return (int)((DateTime.Today - fechaNacimiento).Days / 365.25);
        }
        public JsonResult EmpleadosLLenaronFactoresRiesgos(int? idEmpresa)
        {
            List<PieData> pieDatas = new List<PieData>();
            List<FactorRiesgo> factorRiesgos;
            int otros;

            if (idEmpresa == null)
            {
                factorRiesgos = _context.FactoresRiesgos.ToList();
                otros = _context.Trabajadores.Count() - factorRiesgos.Count();
            }
            else
            {
                factorRiesgos = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(f => f.FactoresRiesgos).ToList();
                otros = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(t => t.FactoresRiesgos).Count() - factorRiesgos.Count();
            }

            string[] lbls = new string[] { "LLenaron Encuesta", "Sin llenar" };
            int[] cnts = new int[]
            {
                factorRiesgos.Count(),
                otros
            };

            PieData pieData = new PieData();
            pieData.Labels = lbls;
            pieData.Counts = cnts;

            pieDatas.Add(pieData);

            return Json(pieData);
        }
        public JsonResult EmpleadosCondicionesConstantes(int? idEmpresa)
        {
            List<FactorRiesgo> factorRiesgos;

            if (idEmpresa == null)
                factorRiesgos = _context.FactoresRiesgos.ToList();
            else
                factorRiesgos = _context.Trabajadores.Where(t => (idEmpresa == null || t.IdEmpresa == idEmpresa))
                    .SelectMany(f => f.FactoresRiesgos).ToList();

            string[] lbls = new string[]
            {
                "Personas vivienda > 3",
                "Hogar con familias multiples",
                "Espacio trabajo Salón",
                "Sin ventilación",
                "Contacto Menor 2mts",
                "Contacto >2hrs"
            };

            int[] cnts = new int[]
                {
                    factorRiesgos.Count(x=>x.NoPersonasCasa >3),
                    factorRiesgos.Count(x=>x.TipoCasa != "1"),
                    factorRiesgos.Where(f => f.TipoTransporte == "Autobús" || f.TipoTransporte == "Metro" || f.TipoTransporte == "Autobus de Empresa" || f.TipoTransporte == "Combi").Count(),
                    factorRiesgos.Count(x=>x.EspacioTrabajo == "Salon" || x.EspacioTrabajo == "Salón"),
                    factorRiesgos.Count(x=>x.TipoVentilacion.Contains("Sin ventilacion")),
                    factorRiesgos.Count(x=>x.ContactoLaboral == "Mantengo una distancia menor de 1.5 metros"),
                    factorRiesgos.Count(x=> x.TiempoContacto == "2" )
                };

            BarChartDatos barChartDatos = new BarChartDatos();
            barChartDatos.Counts = cnts;
            barChartDatos.Labels = lbls;

            return Json(barChartDatos);
        }
        public async Task<JsonResult> EmpleadosRiesgoComplicacionContagio(int? idEmpresa)
        {
            List<FactorRiesgo> factoresRiesgos;

            var CCCM55 = await new FactoresRiesgosController(_context).ComplicacionesCasosContagiosMayor55(idEmpresa);
            var fcccm55 = CCCM55.Where(x => x.Edad > 55);

            if (idEmpresa == null)
                factoresRiesgos = await _context.Trabajadores
                    .SelectMany(f => f.FactoresRiesgos)
                    .ToListAsync();
            else
                factoresRiesgos = await _context.Trabajadores
                    .Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(f => f.FactoresRiesgos).ToListAsync();

            string[] lbls = new string[]
            {
                "Diabetes",
                "Hipertension",
                "Asma",
                "SobrePeso",
                "Obesidad",
                "Embarazo",
                "Enfermedad Autoinmune",
                "Enfermedad Corazon",
                "EPOC",
                "Tabaquismo",
                "Alcoholismo",
                "Drogas",
            };

            int[] cnts = new int[]
                {
                    fcccm55.Count(x=>x.Diabetes == "Si"),
                    fcccm55.Count(x=>x.Hipertension== "Si"),
                    fcccm55.Count(x=>x.Asma== "Si"),
                    fcccm55.Count(x=>x.Sobrepeso== "Si"),
                    fcccm55.Count(x=>x.Obesidad== "Si"),
                    fcccm55.Count(x=>x.Embarazo== "Si"),
                    fcccm55.Count(x=>x.EnfAutoinmune== "Si"),
                    fcccm55.Count(x=>x.EnfermedadCorazon== "Si"),
                    fcccm55.Count(x=>x.EPOC== "Si"),
                    fcccm55.Count(x=>x.Tabaquismo== "Si"),
                    fcccm55.Count(x=>x.Alcoholismo== "Si"),
                    fcccm55.Count(x=>x.Drogas== "Si")
                };

            BarChartDatos barChartDatos = new BarChartDatos();
            barChartDatos.Counts = cnts;
            barChartDatos.Labels = lbls;

            return Json(barChartDatos);
        }
    }
}