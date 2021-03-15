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
using BiPro_Analytics.Responses;
using BiPro_Analytics.UnParo;
using System.Text;

namespace BiPro_Analytics.Controllers
{
    public class SeguimientosCovidController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public SeguimientosCovidController(BiproAnalyticsDBContext context)
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
            ViewBag.Trabajadores = perfilData.DDLTrabajadores ?? new List<DDLTrabajador>();

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                {
                    return NotFound("Datos de empresa no encontrados");
                }
            }

            return View();
        }
        // GET: SeguimientosCovid

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
                    return View(await _context.SeguimientosCovid
                        .Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdUnidad != null)
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {

                if (IdTrabajador != null)
                {
                    return View(await _context.SeguimientosCovid
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {

                    List<SeguimientoCovid> seguimientosCovid = new List<SeguimientoCovid>();

                    seguimientosCovid = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(t => t.SeguimientosCovid).ToListAsync();

                    return View(seguimientosCovid);

                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var seguimientosCovid = await _context.Trabajadores
                        .Where(u => u.IdUnidad == IdUnidad)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync();

                    return View(seguimientosCovid);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(a => a.IdArea == IdArea)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync());
                }
            }
            else
            {
                var seguimientosCovid = await _context.SeguimientosCovid
                    .Where(r => r.IdTrabajador == perfilData.IdTrabajador).ToListAsync();

                if (seguimientosCovid.Count == 0)
                    return NotFound("Trabajador sin seguimiento covid");

                return View(seguimientosCovid);
            }

            return View(await _context.SeguimientosCovid.ToListAsync());

        }

        // GET: SeguimientosCovid/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Create
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            return View();
        }

        // POST: SeguimientosCovid/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EstatusPaciente,SintomasMayores,SintomasMenores,EstatusEnCasa,EstatusEnHospital,FechaSeguimiento,IdTrabajador")] SeguimientoCovid seguimientoCovid)
        {
            if (ModelState.IsValid)
            {
                seguimientoCovid.Trabajador = _context.Trabajadores.Find(seguimientoCovid.IdTrabajador);
                _context.Add(seguimientoCovid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid.FindAsync(id);
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            return View(seguimientoCovid);
        }

        // POST: SeguimientosCovid/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EstatusPaciente,SintomasMayores,SintomasMenores,EstatusEnCasa,EstatusEnHospital,FechaSeguimiento,IdTrabajador")] SeguimientoCovid seguimientoCovid)
        {
            if (id != seguimientoCovid.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    seguimientoCovid.Trabajador = _context.Trabajadores.Find(seguimientoCovid.IdTrabajador);
                    _context.Update(seguimientoCovid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeguimientoCovidExists(seguimientoCovid.Id))
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
            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            return View(seguimientoCovid);
        }

        // POST: SeguimientosCovid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seguimientoCovid = await _context.SeguimientosCovid.FindAsync(id);
            _context.SeguimientosCovid.Remove(seguimientoCovid);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeguimientoCovidExists(int id)
        {
            return _context.SeguimientosCovid.Any(e => e.Id == id);
        }

        /// <summary>
        /// T4.A quienes se deben hacer pruebas para regresar a laborar. Asintomaticos con mas de 2 semanas.
        /// </summary>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        public async Task<IActionResult> PruebasParaRegresoALaborar(int IdTrabajador)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdTrabajador;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var trabajadoresPruebas = await ObtenerTrabajadoresARealizarPruebas(idEmpresa);

            builder.AppendLine("NombreTrabajador,Genero,Telefono,Correo,FechaRegistro,NombreUnidad,NombreArea,EstatusPaciente");

            foreach (var item in trabajadoresPruebas)
            {
                builder.AppendLine($"{item.NombreTrabajador},{item.Genero},{item.Telefono},{item.Correo},{item.FechaRegistro},{item.NombreUnidad},{item.NombreArea},{item.EstatusPaciente}");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptTrabajadoresARealizarPruebas_", dateTime, ".csv"));
        }

        public async Task<List<SeguimientoCovidViewModel>> ObtenerTrabajadoresARealizarPruebas(int idEmpresa)
        {
            var reporteList = new List<SeguimientoCovidViewModel>();

            try
            {
                //T4.A quienes se deben hacer pruebas para regresar a laborar. Asintomaticos con mas de 2 semanas.
                var trabajadoresAsinMas2Sem = await _context.Trabajadores
                    .Where(t => t.IdEmpresa == idEmpresa)
                    .Join(_context.Empresas, tb => tb.IdEmpresa, em => em.IdEmpresa, (tb, em) => new { Trabajador = tb })
                    .Join(_context.SeguimientosCovid, tb => tb.Trabajador.IdTrabajador, sc => sc.IdTrabajador, (tb, sc) => new { sc.EstatusPaciente, sc.EstatusEnCasa, tb })
                    .Where(sc => sc.EstatusEnCasa == 2)
                    .ToListAsync();

                reporteList = trabajadoresAsinMas2Sem.Select(tbA =>
                new SeguimientoCovidViewModel 
                { 
                    NombreTrabajador = tbA.tb.Trabajador.Nombre ,
                    Genero = tbA.tb.Trabajador.Genero,
                    Telefono = tbA.tb.Trabajador.Telefono,
                    Correo = tbA.tb.Trabajador.Correo,
                    FechaRegistro = tbA.tb.Trabajador.FechaRegistro,
                    NombreUnidad = tbA.tb.Trabajador.NombreUnidad ?? string.Empty,
                    NombreArea = tbA.tb.Trabajador.NombreArea ?? string.Empty,
                    EstatusPaciente = tbA.EstatusPaciente
                }).ToList();
            }
            catch (Exception ex)
            { }

            return reporteList;
        }

        public async Task<IActionResult> TrabajadoresSeguimientoGral(int IdTrabajador)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdTrabajador;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var trabajadoresPruebas = await ObtenerTrabajadoresSeguimientoGral(idEmpresa);

            builder.AppendLine("NombreTrabajador,Genero,Telefono,Correo,FechaRegistro,NombreUnidad,NombreArea,EstatusPaciente,SintomasMayores,SintomasMenores,EstatusEnCasa,EstatusEnHospital,FechaSeguimiento");

            foreach (var item in trabajadoresPruebas)
            {
                builder.AppendLine($"{item.NombreTrabajador},{item.Genero},{item.Telefono},{item.Correo},{item.FechaRegistro},{item.NombreUnidad},{item.NombreArea},{item.EstatusPaciente},{item.SintomasMayores},{item.SintomasMenores},{item.EstatusEnCasa},{item.EstatusEnHospital},{item.FechaSeguimiento}");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptTrabajadoresARealizarPruebas_", dateTime, ".csv"));
        }

        public async Task<List<SeguimientoCovidViewModel>> ObtenerTrabajadoresSeguimientoGral(int idEmpresa)
        {
            var reporteList = new List<SeguimientoCovidViewModel>();

            try
            {
                //T5. Seguimiento general de pacientes contagiados. Como continuan, con o sin sintomas, hospitalizados, etc.
                var trabajadoresSeguimiento = await _context.Trabajadores
                    .Where(t => t.IdEmpresa == idEmpresa)
                    .Join(_context.Empresas, tb => tb.IdEmpresa, em => em.IdEmpresa, (tb, em) => new { Trabajador = tb })
                    .Join(_context.SeguimientosCovid, tb => tb.Trabajador.IdTrabajador, sc => sc.IdTrabajador, (tb, sc) => new { SeguimientosCovid = sc, tb })
                    .ToListAsync();

                reporteList = trabajadoresSeguimiento.Select(tbA =>
                new SeguimientoCovidViewModel
                {
                    NombreTrabajador = tbA.tb.Trabajador.Nombre,
                    Genero = tbA.tb.Trabajador.Genero,
                    Telefono = tbA.tb.Trabajador.Telefono,
                    Correo = tbA.tb.Trabajador.Correo,
                    FechaRegistro = tbA.tb.Trabajador.FechaRegistro,
                    NombreUnidad = tbA.tb.Trabajador.NombreUnidad ?? string.Empty,
                    NombreArea = tbA.tb.Trabajador.NombreArea ?? string.Empty,
                    EstatusPaciente = tbA.SeguimientosCovid.EstatusPaciente,
                    SintomasMayores = tbA.SeguimientosCovid.SintomasMayores ? "Si" : "No",
                    SintomasMenores = tbA.SeguimientosCovid.SintomasMenores ? "Si" : "No",
                    EstatusEnCasa = ObtenerStatusEnCasa(tbA.SeguimientosCovid.EstatusEnCasa),
                    EstatusEnHospital = ObtenerStatusEnHospital(tbA.SeguimientosCovid.EstatusEnHospital),
                    FechaSeguimiento = tbA.SeguimientosCovid.FechaSeguimiento
                }).ToList();
            }
            catch (Exception ex)
            { }

            return reporteList;
        }

        private string ObtenerStatusEnCasa(int statusId) 
        {
            string status = string.Empty;
            switch (statusId) 
            {
                case 1:
                    status = "Asintomáticos con menos de 2 semanas";
                    break;
                case 2:
                    status = "Asintomáticos con más de 2 semana";
                    break;
                default:
                    status = string.Empty;
                    break;
            }

            return status;
        }

        private string ObtenerStatusEnHospital(int statusId) 
        {
            string status = string.Empty;
            switch (statusId)
            {
                case 1:
                    status = "Intubado";
                    break;
                case 2:
                    status = "En terapia";
                    break;
                case 3:
                    status = "Dado de alta";
                    break;
                case 4:
                    status = "En aislamiento";
                    break;
                case 5:
                    status = "Falleció";
                    break;
                default:
                    status = string.Empty;
                    break;
            }

            return status;
        }

        public async Task<IActionResult> PacientesNoLlenaronRC(int IdTrabajador)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdTrabajador;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var pacientesNoLLenaronRC = await ObtenerPacientesNoLlenaronRC(idEmpresa);

            builder.AppendLine("NombreTrabajador,Genero,Telefono,Correo,FechaRegistro,NombreUnidad,NombreArea,RealizoPrueba");

            foreach (var item in pacientesNoLLenaronRC)
            {
                builder.AppendLine($"{item.NombreTrabajador},{item.Genero},{item.Telefono},{item.Correo},{item.FechaRegistro},{item.NombreUnidad},{item.NombreArea},{item.YaRealizoPrueba},");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptTrabajadoresNoHanRealizadoRC_", dateTime, ".csv"));
        }

        /// <summary>
        /// //M8. Pacientes que llenaron y no llenaron su F4
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public async Task<List<SeguimientoCovidViewModel>> ObtenerPacientesNoLlenaronRC(int idEmpresa)
        {
            var reporteList = new List<SeguimientoCovidViewModel>();

            try
            {
                var trabajadoresNoLlenaronRC = await _context.Trabajadores
                    .Where(t => t.IdEmpresa == idEmpresa)
                    .Join(_context.Empresas, tb => tb.IdEmpresa, em => em.IdEmpresa, (tb, em) => new { Trabajador = tb })
                    .GroupJoin(_context.Pruebas, tb => tb.Trabajador.IdTrabajador, pb => pb.IdTrabajador, (tb, pb) => new { tb, Pruebas = pb })
                    .SelectMany(x => x.Pruebas.DefaultIfEmpty(), (x, y) => new { Trabajador = x.tb.Trabajador, Prueba = y })
                   .ToListAsync();

                reporteList = trabajadoresNoLlenaronRC.Select(tbA =>
                new SeguimientoCovidViewModel
                {
                    NombreTrabajador = tbA.Trabajador.Nombre,
                    Genero = tbA.Trabajador.Genero,
                    Telefono = tbA.Trabajador.Telefono,
                    Correo = tbA.Trabajador.Correo,
                    FechaRegistro = tbA.Trabajador.FechaRegistro,
                    NombreUnidad = tbA.Trabajador.NombreUnidad ?? string.Empty,
                    NombreArea = tbA.Trabajador.NombreArea ?? string.Empty,
                    YaRealizoPrueba = tbA.Prueba != null ? "Si" : "No"
                }).ToList();
            }
            catch (Exception ex)
            { }

            return reporteList;
        }

        public async Task<IActionResult> QuienNoLlenaronSeguimientoCovidMen(int IdTrabajador)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdTrabajador;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var pacientesNoRealizoSegCOVID = await ObtenerAMNoLlenaronSeguimientoCovidMen(idEmpresa);

            builder.AppendLine("NombreTrabajador,Genero,Telefono,Correo,FechaRegistro,NombreUnidad,NombreArea,RealizoSeguimientoCOVID");

            foreach (var item in pacientesNoRealizoSegCOVID)
            {
                builder.AppendLine($"{item.NombreTrabajador},{item.Genero},{item.Telefono},{item.Correo},{item.FechaRegistro},{item.NombreUnidad},{item.NombreArea},{item.YaRealizoSeguimientoCovid},");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptTrabajadoresNoHanRealizadoRC_", dateTime, ".csv"));
        }

        /// <summary>
        /// M9. AM de seguimiento. Quienes llenaron el seguimiento y queines no en todo el mes.
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public async Task<List<SeguimientoCovidViewModel>> ObtenerAMNoLlenaronSeguimientoCovidMen(int idEmpresa)
        {
            var reporteList = new List<SeguimientoCovidViewModel>();

            try
            {
                var fechaRef = DateTime.Now.AddMonths(-1);
                var anio = fechaRef.Year;
                var month = fechaRef.Month;
                var fechaInicio = new DateTime(anio, month, 1);
                var ultimoDiaMes = DateTime.DaysInMonth(anio, month);
                var fechaFinal = new DateTime(anio, month, ultimoDiaMes);

                var trabajadoresSeguimientoCovidMen = await _context.Trabajadores
                    .Where(t => t.IdEmpresa == idEmpresa)
                    .Join(_context.Empresas, tb => tb.IdEmpresa, em => em.IdEmpresa, (tb, em) => new { Trabajadores = tb })
                    .GroupJoin(_context.SeguimientosCovid, tb => tb.Trabajadores.IdTrabajador, pb => pb.IdTrabajador, (tb, pb) => new { tb, Seguimiento = pb })
                    .SelectMany(x => x.Seguimiento.DefaultIfEmpty(), (x, y) => new { Trabajador = x.tb.Trabajadores, SeguimientoCovid = y })
                    .Where(sc => (sc.SeguimientoCovid != null && sc.SeguimientoCovid.FechaSeguimiento >= fechaInicio && sc.SeguimientoCovid.FechaSeguimiento <= fechaFinal) || sc.SeguimientoCovid == null)
                    .ToListAsync();

                reporteList = trabajadoresSeguimientoCovidMen.Select(tbA =>
                new SeguimientoCovidViewModel
                {
                    NombreTrabajador = tbA.Trabajador.Nombre,
                    Genero = tbA.Trabajador.Genero,
                    Telefono = tbA.Trabajador.Telefono,
                    Correo = tbA.Trabajador.Correo,
                    FechaRegistro = tbA.Trabajador.FechaRegistro,
                    NombreUnidad = tbA.Trabajador.NombreUnidad ?? string.Empty,
                    NombreArea = tbA.Trabajador.NombreArea ?? string.Empty,
                    YaRealizoSeguimientoCovid = tbA.SeguimientoCovid != null ? "Si" : "No"
                }).ToList();
            }
            catch (Exception ex)
            { }

            return reporteList;
        }
    }
}
