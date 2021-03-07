using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BiPro_Analytics.Data;
using BiPro_Analytics.Models;
using BiPro_Analytics.Responses;
using BiPro_Analytics.UnParo;
using BiPro_Analytics.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiPro_Analytics.Controllers
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

        public async Task<IActionResult> PreIndex()
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

            if(idEmpresa == null)
            {
                edadesMujeres = _context.Trabajadores
                .Where(r => r.Genero == "Femenino" ).ToList();

                edadesHombres = _context.Trabajadores
                    .Where(r => r.Genero == "Hombre" ).ToList();
            }
            else
            {
                edadesMujeres = _context.Trabajadores
                .Where(r => r.Genero == "Femenino" && r.IdEmpresa == idEmpresa).ToList();

                edadesHombres = _context.Trabajadores
                    .Where(r => r.Genero == "Hombre" && r.IdEmpresa == idEmpresa).ToList();
            }

            string[] lbls = new string[] { " Covid" };
            int[] cntsMujeres = new int[]
            {
                edadesMujeres.Where(r=>r.Edad > 18 && r.Edad < 25).Count(),
                edadesMujeres.Where(r=>r.Edad > 25 && r.Edad < 35).Count(),
                edadesMujeres.Where(r=>r.Edad > 35 && r.Edad < 45).Count(),
                edadesMujeres.Where(r=>r.Edad > 48 && r.Edad < 55).Count(),
                edadesMujeres.Where(r=>r.Edad > 55 && r.Edad < 65).Count(),
                edadesMujeres.Where(r=>r.Edad > 65 && r.Edad < 75).Count(),
                edadesMujeres.Where(r=>r.Edad > 75 && r.Edad < 85).Count(),
                edadesMujeres.Where(r=>r.Edad > 85).Count()
            };
            int[] cntsHombres = new int[]
            {
                edadesHombres.Where(r=>r.Edad > 18 && r.Edad < 25).Count() * -1,
                edadesHombres.Where(r=>r.Edad > 25 && r.Edad < 35).Count() * -1,
                edadesHombres.Where(r=>r.Edad > 35 && r.Edad < 45).Count() * -1,
                edadesHombres.Where(r=>r.Edad > 48 && r.Edad < 55).Count() * -1,
                edadesHombres.Where(r=>r.Edad > 55 && r.Edad < 65).Count() * -1,
                edadesHombres.Where(r=>r.Edad > 65 && r.Edad < 75).Count() * -1,
                edadesHombres.Where(r=>r.Edad > 75 && r.Edad < 85).Count() * -1,
                edadesHombres.Where(r=>r.Edad > 85 ).Count() * -1
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
                factorRiesgosM = _context.Trabajadores.Where(t => t.Genero == "Femenino" )
                    .SelectMany(f => f.FactoresRiesgos).ToList();
                factorRiesgosH = _context.Trabajadores.Where(t => t.Genero == "Masculino" )
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

            var ObesidadM = factorRiesgosM.Where(x => x.Obesidad).ToList().Count();
            var ObesidadH = factorRiesgosH.Where(x => x.Obesidad).ToList().Count();

            var EmbarazoM = factorRiesgosM.Where(x => x.Embarazo).ToList().Count();


            var CancerM = factorRiesgosM.Where(x => x.Cancer).ToList().Count();
            var CancerH = factorRiesgosH.Where(x => x.Cancer).ToList().Count();

            var TabaquismoM = factorRiesgosM.Where(x => x.Tabaquismo).ToList().Count();
            var TabaquismoH = factorRiesgosH.Where(x => x.Tabaquismo).ToList().Count();

            var AlcoholismoM = factorRiesgosM.Where(x => x.Alcoholismo).ToList().Count();
            var AlcoholismoH = factorRiesgosH.Where(x => x.Alcoholismo).ToList().Count();

            var DrogasM = factorRiesgosM.Where(x => x.Drogas).ToList().Count();
            var DrogasH = factorRiesgosH.Where(x => x.Drogas).ToList().Count();

            int[] cntsMujeres = new int[]
                {
                    diabetesM,
                    ObesidadM,
                    EmbarazoM,
                    CancerM,
                    TabaquismoM,
                    AlcoholismoM,
                    DrogasM
                };

            int[] cntsHombres = new int[]
                {
                    diabetesH,
                    ObesidadH,
                    0,
                    CancerH,
                    TabaquismoH,
                    AlcoholismoH,
                    DrogasH
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

            var mayor3 = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(t => t.FactoresRiesgos).Where(f => f.NoPersonasCasa > 3).Count();
            var multiplesFamilias = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(t => t.FactoresRiesgos).Where(f => f.TipoCasa == "Familias Multiples").Count();
            var transportePublico = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(t => t.FactoresRiesgos).Where(f => f.TipoTransporte == "Publico").Count();

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

            var espacioTrabajo = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(t => t.FactoresRiesgos).Where(f => f.EspacioTrabajo == "Cuarto" || f.EspacioTrabajo == "Salon").Count();
            var tipoVentilacion = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(t => t.FactoresRiesgos).Where(f => f.TipoVentilacion == "Sin ventilacion").Count();

            pieData.Counts = new int[]
            {
                espacioTrabajo,
                espacioTrabajo
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

            var otros = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                .SelectMany(t => t.RiesgosContagios).Count() - riesgoContagios.Count();

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

            if (idEmpresa==null)
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

            var CC = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(f => f.FactoresRiesgos);
            var otros = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                .SelectMany(t => t.RiesgosContagios).Count() - CC.Count();

            string[] lbls = new string[] { "LLenaron Encuesta", "Sin llenar" };
            int[] cnts = new int[]
            {
                CC.Count(),
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
            var factorRiesgos = _context.Trabajadores.Where(t => (idEmpresa == null || t.IdEmpresa == idEmpresa))
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
                    factorRiesgos.Count(x=>x.TipoCasa == "Familias Multiples"),
                    factorRiesgos.Count(x=>x.TipoTransporte == "Publico"),
                    factorRiesgos.Count(x=>x.EspacioTrabajo == "Salon" || x.EspacioTrabajo == "Salón"),
                    factorRiesgos.Count(x=>x.TipoVentilacion == "Sin ventilacion" || x.TipoVentilacion == "Sin ventilacion"),
                    factorRiesgos.Count(x=>x.ContactoLaboral == "Menor 2mts"),
                    factorRiesgos.Count(x=>x.TiempoContacto == "2 hrs")
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