using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiPro_Analytics.Data;
using BiPro_Analytics.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using BiPro_Analytics.Responses;
using BiPro_Analytics.UnParo;
using System.IO;
using CsvHelper;
using System.Text;
using System.Globalization;
using BiPro_Analytics.Responses.FactoresRiesgos;

namespace BiPro_Analytics.Controllers
{
    public class FactoresRiesgosController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public FactoresRiesgosController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PreIndex()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            ViewBag.Unidades = perfilData.DDLUnidades;
            ViewBag.Areas = perfilData.DDLAreas;
            ViewBag.Empresas = perfilData.DDLEmpresas;
            ViewBag.Trabajadores = perfilData.DDLTrabajadores != null ? perfilData.DDLTrabajadores : new List<DDLTrabajador>();

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                {
                    return NotFound("Datos de empresa no encontrados");
                }
                ViewBag.IdEmpresa = perfilData.IdEmpresa;
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ExportEdadGeneroCSV(int idEmpresa)
        {
            var edadGenero = await EdadGenero(idEmpresa);

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,Edad,Genero");

            foreach (var item in edadGenero)
            {
                builder.AppendLine($"{item.Nombre},{item.Edad},{item.Genero}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "EdadGenero.csv");
        }

        [HttpGet]
        public async Task<IActionResult> ExportEnfermedadesCronicasCSV(int idEmpresa)
        {
            var enfermedadesCronicas = await EnfermedadesCronicas(idEmpresa);

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,Diabetes,Hipertension,Asma,Sobrepeso,Obesidad,EnfAutoinmune,EnfermedadCorazon,Cancer,EPOC,Embarazo");

            foreach (var item in enfermedadesCronicas)
            {
                builder.AppendLine($"{item.Nombre},{item.Diabetes},{item.Hipertension},{item.Asma},{item.Sobrepeso},{item.Obesidad},{item.EnfAutoinmune},{item.EnfermedadCorazon},{item.Cancer},{item.EPOC},{item.Embarazo}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "ExportEnfermedadesCronicas.csv");
        }

        public async Task<IActionResult> ExportAPNPCSV(int idEmpresa)
        {
            
            var apnp = await APNP(idEmpresa);

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,Tabaquismo,Consumo Alcohol,FarmacosDrogas");

            foreach (var item in apnp)
            {
                builder.AppendLine($"{item.Nombre},{item.Tabaquismo},{item.Alcoholismo},{item.Drogas}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "APNP.csv");
        }

        public async Task<IActionResult> ExportRiesgosCasaTransporte(int idEmpresa)
        {
            
            var riesgosCasaTransporte = await RiesgosCasaTransportes(idEmpresa);

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,No_Personas_Casa,Tipo_Casa,Tipo_Transporte");

            foreach (var item in riesgosCasaTransporte)
            {
                builder.AppendLine($"{item.Nombre},{item.NoPersonasCasa},{item.TipoCasa},{item.TipoTransporte}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "RiesgosCasaTransporte.csv");

        }

        public async Task<IActionResult> ExportRiesgosLaboral(int idEmpresa)
        {
            
            var riesgoLaborales = await RiesgoLaborales(idEmpresa);

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,Espacio de trabajo,Tipo de ventilación,Contacto Laboral, Tiempo de contacto");

            foreach (var item in riesgoLaborales)
            {
                builder.AppendLine($"{item.Nombre},{item.EspacioTrabajo},{item.TipoVentilacion},{item.ContactoLaboral},{item.TiempoContacto}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "RiesgosEnEspacioLaboral.csv");

        }
        public async Task<IActionResult> ExportLlenadoFactoresRiesgos(int idEmpresa)
        {
            
            var EmpleadosFactoresRiesgos = await _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).SelectMany(x => x.FactoresRiesgos).ToListAsync();

            var factoresRiesgos = await _context.FactoresRiesgos.ToListAsync();

            var trabajadores = await _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa).ToListAsync();



            var LlenadoFactoresRiesgos = trabajadores.Select(x => new LlenadoFactoresdeRiesgos
            {
                Nombre = x.Nombre,
                LlenadoEncuesta = factoresRiesgos.Exists(y => y.IdTrabajador == x.IdTrabajador) ? "Si" : "No"
            });

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,LlenadoEncuesta");

            foreach (var item in LlenadoFactoresRiesgos)
            {
                builder.AppendLine($"{item.Nombre},{item.LlenadoEncuesta}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "LlenadodeFactoresdeRiesgos.csv");
        }
        public async Task<IActionResult> ExportCondicionesConstantesRiesgosContagios(int idEmpresa)
        {
            
            var condicionesCRC = await CondicionesConstantesRiesgosContagios(idEmpresa);

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,EspacioTrabajo,TipoVentilacion,ContactoLaboral,TiempoContacto,EspacioTrabajo,TipoVentilacion,ContactoLaboral,TiempoContacto");

            foreach (var item in condicionesCRC)
            {
                builder.AppendLine($"{item.Nombre},{item.EspacioTrabajo},{item.TipoVentilacion},{item.ContactoLaboral},{item.TiempoContacto},{item.EspacioTrabajo},{item.TipoVentilacion},{item.ContactoLaboral},{item.TiempoContacto}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "CondicionesConstantesRiesgosContagios.csv");

        }
        public async Task<IActionResult> ExportComplicacionCasoContagioMayor55(int idEmpresa)
        {
            
            var ComplicacionCCM55 = await ComplicacionesCasosContagiosMayor55(idEmpresa);

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,Edad,Genero,Diabetes,Hipertension,Asma,Sobrepeso,Obesidad,Embarazo,Enfermedad_Autoinmune,Enfermedad_Corazon,EPOC,Tabaquismo,Alcoholismo,Drogas");

            foreach (var item in ComplicacionCCM55)
            {
                builder.AppendLine($"{item.Nombre},{item.Edad},{item.Genero},{item.Diabetes},{item.Hipertension},{item.Asma},{item.Sobrepeso},{item.Obesidad},{item.Embarazo},{item.EnfAutoinmune},{item.EnfermedadCorazon},{item.EPOC},{item.Tabaquismo},{item.Alcoholismo},{item.Drogas}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "ComplicacionCasoContagioMayor55.csv");

        }

        // GET: FactoresRiesgos
        public async Task<IActionResult> Index(int? IdTrabajador, int? IdUnidad, int? IdArea)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            ViewBag.Unidades = perfilData.DDLUnidades;
            ViewBag.Areas = perfilData.DDLAreas;
            ViewBag.Empresas = perfilData.DDLEmpresas;
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                    return NotFound("Usuario no asociado a ninguna empresa");
            }

            if (currentUser.IsInRole("Admin"))
            {
                if (IdTrabajador != null)
                    return View(await _context.FactoresRiesgos
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .Include(x=>x.Trabajador)
                        .ToListAsync());

                if (IdUnidad != null && IdArea == null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad)
                        .SelectMany(r => r.FactoresRiesgos)
                        .Include(x => x.Trabajador)
                        .ToListAsync());

                if (IdUnidad == null && IdArea != null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdArea == IdArea)
                        .SelectMany(r => r.FactoresRiesgos)
                        .Include(x => x.Trabajador)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea)
                        .SelectMany(r => r.FactoresRiesgos)
                        .Include(x => x.Trabajador)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (IdTrabajador != null)
                {
                    return View(await _context.FactoresRiesgos
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .Include(x => x.Trabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {

                    List<FactorRiesgo> factoresRiesgos = new List<FactorRiesgo>();

                    factoresRiesgos = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(t => t.FactoresRiesgos)
                        .Include(x => x.Trabajador)
                        .ToListAsync();

                    return View(factoresRiesgos);

                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.FactoresRiesgos)
                        .Include(x => x.Trabajador)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var riesgos = await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.FactoresRiesgos)
                        .Include(x => x.Trabajador)
                        .ToListAsync();

                    return View(riesgos);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdArea == IdArea && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.FactoresRiesgos)
                        .Include(x => x.Trabajador)
                        .ToListAsync());
                }
            }
            else
            {
                var factoresRiesgos = await _context.FactoresRiesgos
                    .Where(r => r.IdTrabajador == perfilData.IdTrabajador)
                    .Include(x => x.Trabajador)
                    .ToListAsync();

                if (factoresRiesgos.Count == 0)
                    return NotFound();

                return View(factoresRiesgos);
            }

            return View(await _context.FactoresRiesgos
                .Include(x => x.Trabajador)
                .ToListAsync());
        }

        // GET: FactoresRiesgos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factorRiesgo = await _context.FactoresRiesgos
                .Include(x => x.Trabajador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factorRiesgo == null)
            {
                return NotFound();
            }

            return View(factorRiesgo);
        }

        // GET: FactoresRiesgos/Create
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            return View();
        }

        // POST: FactoresRiesgos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Diabetes,Hipertension,Asma,SobrePeso,Obesidad,EnfermedadAutoinmune,EnfermedadCorazon,EPOC,Embarazo,Cancer,Tabaquismo,Alcoholismo,Drogas,NoPersonasCasa,TipoCasa,TipoTransporte,EspacioTrabajo,TipoVentilacion,ContactoLaboral,TiempoContacto,IdTrabajador")] FactorRiesgo factorRiesgo)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (currentUser.IsInRole("Trabajador"))
            {
                factorRiesgo.Trabajador = _context.Trabajadores.Find(perfilData.IdTrabajador);
                factorRiesgo.IdTrabajador = perfilData.IdTrabajador;
            }
            else
            {
                factorRiesgo.Trabajador = _context.Trabajadores.Find(factorRiesgo.IdTrabajador);
                factorRiesgo.IdTrabajador = factorRiesgo.IdTrabajador;
            }

            if (ModelState.IsValid)
            {

                factorRiesgo.FechaHoraRegistro = DateTime.Now;

                _context.Add(factorRiesgo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(factorRiesgo);
        }

        // GET: FactoresRiesgos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factorRiesgo = await _context.FactoresRiesgos.FindAsync(id);
            if (factorRiesgo == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            return View(factorRiesgo);
        }

        // POST: FactoresRiesgos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Diabetes,Hipertension,Asma,SobrePeso,Obesidad,EnfermedadAutoinmune,EnfermedadCorazon,EPOC,Embarazo,Cancer,Tabaquismo,Alcoholismo,Drogas,NoPersonasCasa,TipoCasa,TipoTransporte,EspacioTrabajo,TipoVentilacion,ContactoLaboral,TiempoContacto,IdTrabajador")] FactorRiesgo factorRiesgo)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (id != factorRiesgo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (currentUser.IsInRole("Trabajador"))
                    {
                        factorRiesgo.Trabajador = _context.Trabajadores.Find(perfilData.IdTrabajador);
                        factorRiesgo.IdTrabajador = perfilData.IdTrabajador;
                    }
                        

                    else
                        factorRiesgo.Trabajador = _context.Trabajadores.Find(factorRiesgo.IdTrabajador);

                    factorRiesgo.FechaHoraRegistro = DateTime.Now;
                    _context.Update(factorRiesgo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactorRiesgoExists(factorRiesgo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(factorRiesgo);
        }

        // GET: FactoresRiesgos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factorRiesgo = await _context.FactoresRiesgos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factorRiesgo == null)
            {
                return NotFound();
            }

            return View(factorRiesgo);
        }

        // POST: FactoresRiesgos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var factorRiesgo = await _context.FactoresRiesgos.FindAsync(id);
            _context.FactoresRiesgos.Remove(factorRiesgo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactorRiesgoExists(int id)
        {
            return _context.FactoresRiesgos.Any(e => e.Id == id);
        }

        private async Task<IEnumerable<EdadGenero>> EdadGenero(int IdEmpresa)
        {
            var edadGenero = await _context.Trabajadores
                .Where(t => t.IdEmpresa == IdEmpresa)
                .Select(t => new EdadGenero
                {
                    Nombre = t.Nombre,
                    Edad = (int)((DateTime.Today - t.FechaNacimiento).Days / 365.25),
                    Genero = t.Genero
                }).ToListAsync();

            return edadGenero;
        }
        private async Task<IEnumerable<EnfermedadesCronicas>> EnfermedadesCronicas(int IdEmpresa)
        {
            var enfsCronicas = await _context.Trabajadores
                .Where(t => t.IdEmpresa == IdEmpresa)
                .SelectMany(f => f.FactoresRiesgos)
                .Select(f => new EnfermedadesCronicas
                {
                    Nombre = _context.Trabajadores.Where(x => x.IdTrabajador == f.IdTrabajador).Select(y => y.Nombre).FirstOrDefault(),
                    Diabetes = f.Diabetes ? "Si" : "No",
                    Hipertension = f.Hipertension ? "Si" : "No",
                    Asma = f.Asma ? "Si" : "No",
                    Embarazo = f.Embarazo ? "Si" : "No",
                    Sobrepeso = f.SobrePeso ? "Si" : "No",
                    Obesidad = f.Obesidad ? "Si" : "No",
                    EnfAutoinmune = f.EnfermedadAutoinmune ? "Si" : "No",
                    EnfermedadCorazon = f.EnfermedadCorazon ? "Si" : "No",
                    EPOC = f.EPOC ? "Si" : "No"
                }).ToListAsync();

            return enfsCronicas;
        }
        private async Task<IEnumerable<APNP>> APNP(int IdEmpresa)
        {
            var aPNP = await _context.Trabajadores
                .Where(t => t.IdEmpresa == IdEmpresa)
                .SelectMany(f => f.FactoresRiesgos)
                .Select(f => new APNP
                {
                    Nombre = _context.Trabajadores.Where(x => x.IdTrabajador == f.IdTrabajador).Select(y => y.Nombre).FirstOrDefault(),
                    Tabaquismo = f.Tabaquismo ? "Si" : "No",
                    Alcoholismo = f.ConsumoAlcohol? "Si" : "No",
                    Drogas = f.FarmacosDrogas ? "Si" : "No"
                }).ToListAsync();

            return aPNP;
        }
        private async Task<IEnumerable<RiesgosCasaTransporte>> RiesgosCasaTransportes(int IdEmpresa)
        {
            var riesgosCasaTransporte = await _context.Trabajadores
                .Where(t => t.IdEmpresa == IdEmpresa)
                .SelectMany(f => f.FactoresRiesgos)
                .Select(f => new RiesgosCasaTransporte
                {
                    Nombre = _context.Trabajadores.Where(x => x.IdTrabajador == f.IdTrabajador).Select(y => y.Nombre).FirstOrDefault(),
                    NoPersonasCasa = f.NoPersonasCasa,
                    TipoCasa = f.TipoCasa,
                    TipoTransporte = f.TipoTransporte

                }).ToListAsync();

            return riesgosCasaTransporte;
        }
        private async Task<IEnumerable<RiesgoLaboral>> RiesgoLaborales(int IdEmpresa)
        {
            var riesgosLaborales = await _context.Trabajadores
                .Where(t => t.IdEmpresa == IdEmpresa)
                .SelectMany(f => f.FactoresRiesgos)
                .Select(f => new RiesgoLaboral
                {
                    Nombre = _context.Trabajadores.Where(x => x.IdTrabajador == f.IdTrabajador).Select(y => y.Nombre).FirstOrDefault(),
                    ContactoLaboral = f.ContactoLaboral,
                    EspacioTrabajo = f.EspacioTrabajo,
                    TiempoContacto = f.TiempoContacto.ToString(),
                    TipoVentilacion = f.TipoVentilacion
                }).ToListAsync();

            return riesgosLaborales;

        }
        private async Task<IEnumerable<CondicionesConstantesRiesgoContagio>> CondicionesConstantesRiesgosContagios(int IdEmpresa)
        {
            var CondicionesCRContagios = await _context.Trabajadores
                .Where(t => t.IdEmpresa == IdEmpresa)
                .SelectMany(f => f.FactoresRiesgos)
                .Select(f => new CondicionesConstantesRiesgoContagio
                {
                    Nombre = _context.Trabajadores.Where(x => x.IdTrabajador == f.IdTrabajador).Select(y => y.Nombre).FirstOrDefault(),
                    NoPersonasCasa = f.NoPersonasCasa,
                    TipoCasa = f.TipoCasa,
                    TipoTransporte = f.TipoTransporte,
                    ContactoLaboral = f.ContactoLaboral,
                    EspacioTrabajo = f.EspacioTrabajo,
                    TiempoContacto = f.TiempoContacto.ToString(),
                    TipoVentilacion = f.TipoVentilacion
                }).ToListAsync();

            return CondicionesCRContagios;
        }
        public async Task<IEnumerable<ComplicacionCasoContagioMayor55>> ComplicacionesCasosContagiosMayor55(int? IdEmpresa)
        {
            List<ComplicacionCasoContagioMayor55> ComplicacionCasoContagioM55;

            if (IdEmpresa == null)
            {
                ComplicacionCasoContagioM55 = await _context.FactoresRiesgos
                .Select(f => new ComplicacionCasoContagioMayor55
                {
                    Nombre = _context.Trabajadores.Where(x => x.IdTrabajador == f.IdTrabajador).Select(y => y.Nombre).FirstOrDefault(),
                    Edad = _context.Trabajadores.Where(x => x.IdTrabajador == f.IdTrabajador).Select(y => y.Edad).FirstOrDefault(),
                    Diabetes = f.Diabetes ? "Si" : "No",
                    Hipertension = f.Hipertension ? "Si" : "No",
                    Asma = f.Asma ? "Si" : "No",
                    Embarazo = f.Embarazo ? "Si" : "No",
                    Sobrepeso = f.SobrePeso ? "Si" : "No",
                    Obesidad = f.Obesidad ? "Si" : "No",
                    EnfAutoinmune = f.EnfermedadAutoinmune ? "Si" : "No",
                    EnfermedadCorazon = f.EnfermedadCorazon ? "Si" : "No",
                    EPOC = f.EPOC ? "Si" : "No",
                    Tabaquismo = f.Tabaquismo ? "Si" : "No",
                    Alcoholismo = f.ConsumoAlcohol? "Si" : "No",
                    Drogas = f.ConsumoAlcohol ? "Si" : "No"

                }).ToListAsync();
            }
            else
            {
                ComplicacionCasoContagioM55 = await _context.Trabajadores
                .Where(t => t.IdEmpresa == IdEmpresa)
                .SelectMany(f => f.FactoresRiesgos)
                .Select(f => new ComplicacionCasoContagioMayor55
                {
                    Nombre = _context.Trabajadores.Where(x => x.IdTrabajador == f.IdTrabajador).Select(y => y.Nombre).FirstOrDefault(),
                    Edad = _context.Trabajadores.Where(x => x.IdTrabajador == f.IdTrabajador).Select(y => y.Edad).FirstOrDefault(),
                    Diabetes = f.Diabetes ? "Si" : "No",
                    Hipertension = f.Hipertension ? "Si" : "No",
                    Asma = f.Asma ? "Si" : "No",
                    Embarazo = f.Embarazo ? "Si" : "No",
                    Sobrepeso = f.SobrePeso ? "Si" : "No",
                    Obesidad = f.Obesidad ? "Si" : "No",
                    EnfAutoinmune = f.EnfermedadAutoinmune ? "Si" : "No",
                    EnfermedadCorazon = f.EnfermedadCorazon ? "Si" : "No",
                    EPOC = f.EPOC ? "Si" : "No",
                    Tabaquismo = f.Tabaquismo ? "Si" : "No",
                    Alcoholismo = f.ConsumoAlcohol ? "Si" : "No",
                    Drogas = f.FarmacosDrogas ? "Si" : "No"

                }).ToListAsync();
            }

            return ComplicacionCasoContagioM55.Where(x => x.Edad > 55);
        }

    }
}