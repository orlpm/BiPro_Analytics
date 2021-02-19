using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BiPro_Analytics.Data;
using BiPro_Analytics.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Charts()
        {
            return View();
        }

        public JsonResult PiramidePoblacional()
        {
            List<PiramidePoblacional> piramidePoblacionallist = new List<PiramidePoblacional>();

            var edad1825 = _context.Trabajadores.Where(r => r.Edad > 18 && r.Edad < 25 && r.Genero == "Femenino")
                .ToList().Count();
            var edad2535 = _context.Trabajadores.Where(r => r.Edad > 25 && r.Edad < 35 && r.Genero == "Femenino")
                .ToList().Count();
            var edad3545 = _context.Trabajadores.Where(r => r.Edad > 35 && r.Edad < 45 && r.Genero == "Femenino")
                .ToList().Count();
            var edad4555 = _context.Trabajadores.Where(r => r.Edad > 45 && r.Edad < 55 && r.Genero == "Femenino")
                .ToList().Count();
            var edad5565 = _context.Trabajadores.Where(r => r.Edad > 55 && r.Edad < 65 && r.Genero == "Femenino")
                .ToList().Count();
            var edad6575 = _context.Trabajadores.Where(r => r.Edad > 65 && r.Edad < 75 && r.Genero == "Femenino")
                .ToList().Count();
            var edad7585 = _context.Trabajadores.Where(r => r.Edad > 75 && r.Edad < 85 && r.Genero == "Femenino")
                .ToList().Count();
            var edad85 = _context.Trabajadores.Where(r => r.Edad > 85 && r.Genero == "Femenino")
                .ToList().Count();

            var edad1825Hombre = _context.Trabajadores.Where(r => r.Edad > 18 && r.Edad < 25 && r.Genero == "Masculino")
                .ToList().Count();
            var edad2535Hombre = _context.Trabajadores.Where(r => r.Edad > 25 && r.Edad < 35 && r.Genero == "Masculino")
                .ToList().Count();
            var edad3545Hombre = _context.Trabajadores.Where(r => r.Edad > 35 && r.Edad < 45 && r.Genero == "Masculino")
                .ToList().Count();
            var edad4555Hombre = _context.Trabajadores.Where(r => r.Edad > 45 && r.Edad < 55 && r.Genero == "Masculino")
                .ToList().Count();
            var edad5565Hombre = _context.Trabajadores.Where(r => r.Edad > 55 && r.Edad < 65 && r.Genero == "Masculino")
                .ToList().Count();
            var edad6575Hombre = _context.Trabajadores.Where(r => r.Edad > 65 && r.Edad < 75 && r.Genero == "Masculino")
                .ToList().Count();
            var edad7585Hombre = _context.Trabajadores.Where(r => r.Edad > 75 && r.Edad < 85 && r.Genero == "Masculino")
                .ToList().Count();
            var edad85Hombre = _context.Trabajadores.Where(r => r.Edad > 85 && r.Genero == "Masculino")
                .ToList().Count();

            string[] lbls = new string[] { " Covid" };
            int[] cntsMujeres = new int[]
            {
                edad1825, 
                edad2535, 
                edad3545, 
                edad4555, 
                edad5565, 
                edad6575, 
                edad7585, 
                edad85
            };
            int[] cntsHombres = new int[]
            { 
                edad1825Hombre * -1, 
                edad2535Hombre * -1, 
                edad3545Hombre * -1, 
                edad4555Hombre * -1, 
                edad5565Hombre * -1, 
                edad6575Hombre * -1, 
                edad7585Hombre * -1, 
                edad85Hombre * -1 
            };

            PiramidePoblacional piramidePoblacional = new PiramidePoblacional();
            piramidePoblacional.Labels = lbls;
            piramidePoblacional.CountsMujeres = cntsMujeres;
            piramidePoblacional.CountsHombres = cntsHombres;

            piramidePoblacionallist.Add(piramidePoblacional);

            return Json(piramidePoblacional);
        }

        public JsonResult CondicionesRiesgo()
        {
            List<CondicionesRiesgo> condicionesRiesgos = new List<CondicionesRiesgo>();

            //var diabetesM = _context.FactoresRiesgos.Where(f => f.Diabetes).Count();

            var M = _context.Trabajadores.Where(t => t.Genero == "Femenino").ToList();

            var factorRiesgos = _context.Trabajadores.Where(t => t.Genero == "Femenino")
                .SelectMany(f => f.FactoresRiesgos);

            var diabetesM = _context.Trabajadores.Where(t => t.Genero == "Femenino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Diabetes).ToList().Count();
            var diabetesH = _context.Trabajadores.Where(t => t.Genero == "Masculino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Diabetes).ToList().Count();

            var ObesidadM = _context.Trabajadores.Where(t => t.Genero == "Femenino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Obesidad).ToList().Count();
            var ObesidadH = _context.Trabajadores.Where(t => t.Genero == "Masculino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Obesidad).ToList().Count();

            var EmbarazoM = _context.Trabajadores.Where(t => t.Genero == "Femenino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Embarazo).ToList().Count();
            

            var CancerM = _context.Trabajadores.Where(t => t.Genero == "Femenino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Cancer).ToList().Count();
            var CancerH = _context.Trabajadores.Where(t => t.Genero == "Masculino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Cancer).ToList().Count();

            var TabaquismoM = _context.Trabajadores.Where(t => t.Genero == "Femenino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Tabaquismo).ToList().Count();
            var TabaquismoH = _context.Trabajadores.Where(t => t.Genero == "Masculino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Tabaquismo).ToList().Count();

            var AlcoholismoM = _context.Trabajadores.Where(t => t.Genero == "Femenino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Alcoholismo).ToList().Count();
            var AlcoholismoH = _context.Trabajadores.Where(t => t.Genero == "Masculino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Alcoholismo).ToList().Count();

            var DrogasM = _context.Trabajadores.Where(t => t.Genero == "Femenino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Drogas).ToList().Count();
            var DrogasH = _context.Trabajadores.Where(t => t.Genero == "Masculino")
                .SelectMany(f => f.FactoresRiesgos).Where(x => x.Drogas).ToList().Count();

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

        public JsonResult RiesgosExpocisionCasaTransporte()
        {
            List<PieData> pieDatas = new List<PieData>();
            PieData pieData = new PieData();

            var mayor3 = _context.FactoresRiesgos.Where(f => f.NoPersonasCasa > 3).Count();
            var multiplesFamilias = _context.FactoresRiesgos.Where(f => f.TipoCasa == "Familias Multiples").Count();
            var transportePublico = _context.FactoresRiesgos.Where(f => f.TipoTransporte == "Publico").Count();

            pieData.Counts = new int[]
            {
                mayor3,
                multiplesFamilias,
                transportePublico
            };

            return Json(pieData);
        }

        public JsonResult RiesgosEspacioLaboral()
        {
            List<PieData> pieDatas = new List<PieData>();
            PieData pieData = new PieData();

            var espacioTrabajo = _context.FactoresRiesgos.Where(f => f.EspacioTrabajo == "Cuarto" || f.EspacioTrabajo == "Salon").Count();
            var tipoVentilacion = _context.FactoresRiesgos.Where(f => f.TipoVentilacion == "Sin ventilacion").Count();

            pieData.Counts = new int[]
            {
                espacioTrabajo,
                espacioTrabajo
            };

            return Json(pieData);
        }


        [HttpGet]
        public JsonResult EmpleadosContactoCovid()
        {
            List<PieData> pieDatas = new List<PieData>();

            var CC = _context.RiesgoContagios.Where(r => r.ContactoCovidCasa || r.ContactoCovidFuera || r.ContactoCovidTrabajo).ToList();
            var otros = _context.RiesgoContagios.Count() - CC.Count();

            string[] lbls = new string[] { "Contacto Covid", "Otros" };
            int[] cnts = new int[]
            {
                CC.Count,
                otros
            };

            PieData pieData = new PieData();
            pieData.Labels = lbls;
            pieData.Counts = cnts;

            pieDatas.Add(pieData);

            return Json(pieData);
        }

        public JsonResult SintomasCOVID()
        {
            List<PieData> pieDatas = new List<PieData>();
            PieData pieData = new PieData();

            var sintomasCovid = _context.RiesgoContagios
                .Where(r=>r.Diarrea || r.DificultadRespirar || r.DolorMuscular || r.Escalofrios 
                                    || r.NauseaVomito || r.Resfriado || r.TempMayor38 || r.Tos || r.TosRecurrente ).Count();

            var otros = _context.RiesgoContagios.Count() - sintomasCovid;

            pieData.Counts = new int[]
            {
                sintomasCovid,
                otros
            };

            return Json(pieData);
        }

    }
}
