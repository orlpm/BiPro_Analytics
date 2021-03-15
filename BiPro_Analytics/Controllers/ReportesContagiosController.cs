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
    public class ReportesContagiosController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public ReportesContagiosController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PreIndex(int? IdUnidad, int? IdArea)
        {
            ClaimsPrincipal currentUser = this.User;
            Empresa empresa = null;
            UsuarioTrabajador usuarioTrabajador = null;
            List<DDLTrabajador> trabajadores = null;
            List<Unidad> unidades = null;
            List<Area> areas = null;
            List<DDLEmpresa> empresas = null;

            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUserId != null)
                usuarioTrabajador = await _context.UsuariosTrabajadores
                    .FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));

            if (usuarioTrabajador != null)
                empresa = await _context.Empresas
                    .FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);

            if (currentUser.IsInRole("Admin"))
            {
                trabajadores = _context.Trabajadores
                    .Select(x => new DDLTrabajador
                    {
                        Id = x.IdTrabajador,
                        Trabajador = x.Nombre
                    }).ToList();

                ViewBag.Trabajadores = trabajadores;

                //unidades = await _context.Unidades.Where(u => u.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                unidades = await _context.Unidades.ToListAsync();
                areas = await _context.Areas.ToListAsync();
                empresas = await _context.Empresas.Select(t => new DDLEmpresa { Id = t.IdEmpresa, Empresa = t.Nombre }).ToListAsync();

                ViewBag.Unidades = unidades;
                ViewBag.areas = areas;
                ViewBag.Empresas = empresas;
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (empresa != null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == empresa.IdEmpresa)
                        .Select(x => new DDLTrabajador
                        {
                            Id = x.IdTrabajador,
                            Trabajador = x.Nombre
                        }).ToListAsync();

                    ViewBag.Trabajadores = trabajadores;

                    unidades = await _context.Unidades.Where(u => u.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                    ViewBag.Unidades = unidades;

                    if (IdUnidad != null)
                    {
                        ViewBag.Unidad = IdUnidad;
                    }

                    empresas.Add(new DDLEmpresa { Id = empresa.IdEmpresa, Empresa = empresa.Nombre });
                    areas = await _context.Areas.Where(a => a.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                    ViewBag.Areas = areas;
                    ViewBag.Empresas = empresas;

                }
                else
                {
                    return NotFound("Datos de empresa no encontrados");
                }
            }
            else if (currentUser.IsInRole("Trabajador"))
            {
                if (usuarioTrabajador != null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdTrabajador == usuarioTrabajador.TrabajadorId)
                        .Select(x => new DDLTrabajador
                        {
                            Id = x.IdTrabajador,
                            Trabajador = x.Nombre
                        }).ToListAsync();

                    unidades = await _context.Unidades.Where(u => u.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                    areas = await _context.Areas.Where(a => a.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                    empresas.Add(new DDLEmpresa { Id = empresa.IdEmpresa, Empresa = empresa.Nombre });

                    ViewBag.Trabajadores = trabajadores;
                    ViewBag.Unidades = unidades;
                    ViewBag.areas = areas;
                    ViewBag.Empresas = empresas;
                }
                else
                {
                    return NotFound("Usuario no vinculado a trabajador");
                }
            }

            return View();
        }

        // GET: ReportesContagios
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReporteContagio.ToListAsync());
        }

        // GET: ReportesContagios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reporteContagio = await _context.ReporteContagio
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reporteContagio == null)
            {
                return NotFound();
            }

            return View(reporteContagio);
        }

        // GET: ReportesContagios/Create
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;
            ViewBag.Empresas =  perfilData.DDLEmpresas;
            ViewBag.Areas = perfilData.DDLAreas;

            return View();
        }

        // POST: ReportesContagios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PositivosSemPCR,PositivosSemLG,PositivosSemAntigeno,PositivosSemTAC,PositivosSemNeumoniaNoConfirmadaCOVID,PositivosSospechososNeumoniaNoConfirmadaCOVID,SospechososDescartados,FechaRegistro,IdEmpresa,IdArea")] ReporteContagio reporteContagio)
        {
            if (ModelState.IsValid)
            {
                reporteContagio.Empresa = _context.Empresas.Find(reporteContagio.IdEmpresa);
                _context.Add(reporteContagio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var areas = _context.Areas.Where(a => a.IdEmpresa == reporteContagio.IdEmpresa).ToList();
            ViewBag.Areas = areas.Select(a => new DDLArea { Id = a.Id, Area = a.Name }).ToList();
            return View(reporteContagio);
        }

        // GET: ReportesContagios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reporteContagio = await _context.ReporteContagio.FindAsync(id);
            if (reporteContagio == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;
            ViewBag.Empresas = perfilData.DDLEmpresas;

            return View(reporteContagio);
        }

        // POST: ReportesContagios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Semana,Anio,FechaRegistro,PositivosSemPCR,PositivosSemLG,PositivosSemAntigeno,PositivosSemTAC,PositivosSemNeumoniaNoConfirmadaCOVID,PositivosSospechososNeumoniaNoConfirmadaCOVID,SospechososDescartados,IdEmpresa")] ReporteContagio reporteContagio)
        {
            if (id != reporteContagio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reporteContagio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReporteContagioExists(reporteContagio.Id))
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
            return View(reporteContagio);
        }

        // GET: ReportesContagios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reporteContagio = await _context.ReporteContagio
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reporteContagio == null)
            {
                return NotFound();
            }

            return View(reporteContagio);
        }

        // POST: ReportesContagios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reporteContagio = await _context.ReporteContagio.FindAsync(id);
            _context.ReporteContagio.Remove(reporteContagio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReporteContagioExists(int id)
        {
            return _context.ReporteContagio.Any(e => e.Id == id);
        }

        public async Task<IActionResult> PositivosSospechososSemanales(int IdEmpresa)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdEmpresa;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var reporteContagios = await ObtenerReporteContagioSemanal(idEmpresa);

            builder.AppendLine("Empresa,Tipo,Cantidad");

            foreach (var item in reporteContagios)
            {
                builder.AppendLine($"{item.NombreEmpresa},{item.Tipo},{item.Cantidad}");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptContagioPositivoSemanal_", dateTime, ".csv"));
        }

        public async Task<List<ReporteContagioViewModel>> ObtenerReporteContagioSemanal(int IdEmpresa)
        {
            var reporteList = new List<ReporteContagioViewModel>();

            try
            {
                var nombreEmpresa = _context.Empresas.Where(x => x.IdEmpresa == IdEmpresa).Select(y => y.Nombre).FirstOrDefault();

                var fechaActual = DateTime.Now.Date;
                var lunesSemanaPasada = fechaActual.AddDays(-(int)fechaActual.DayOfWeek - 6 - 7);
                var fechaFinal = lunesSemanaPasada.AddDays(6);

                var rcPosSemanalesAntigeno = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSemAntigeno)
                    .Select(group => group.Sum(item => item.PositivosSemAntigeno))
                    //.OrderBy(n => n.PosSemAntigeno)
                    .ToListAsync();

                var posSemAntigeno =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    TipoId = "rcPosSemanalesAntigeno",
                    Tipo = "Positivos Semanales Antigeno",
                    Cantidad = rcPosSemanalesAntigeno.Sum()
                };
                reporteList.Add(posSemAntigeno);

                var rcPosSemanalesLG = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSemLG)
                    .Select(group => group.Sum(item => item.PositivosSemLG))
                    //.OrderBy(n => n.PosSemLG)
                    .ToListAsync();

                var posSemanalesLG =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales LG",
                    TipoId = "rcPosSemanalesLG",
                    Cantidad = rcPosSemanalesLG.Sum()
                };
                reporteList.Add(posSemanalesLG);

                var rcPosSemNeumoniaNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSemNeumoniaNoConfirmadaCOVID)
                    .Select(group => group.Sum(item => item.PositivosSemNeumoniaNoConfirmadaCOVID))
                    //.OrderBy(n => n.PosSemNeumoniaNoConfCOVID)
                    .ToListAsync();

                var posSemNeumoniaNoConfCOVID =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales Neumonia No confirmada Covid",
                    TipoId = "rcPosSemNeumoniaNoConfCOVID",
                    Cantidad = rcPosSemNeumoniaNoConfCOVID.Sum()
                };
                reporteList.Add(posSemNeumoniaNoConfCOVID);

                var rcPositivosSemPCR = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSemPCR)
                    .Select(group => group.Sum(item => item.PositivosSemPCR))
                    //.OrderBy(n => n.PositivosSemPCR)
                    .ToListAsync();

                var positivosSemPCR =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales PCR",
                    TipoId = "rcPositivosSemPCR",
                    Cantidad = rcPositivosSemPCR.Sum()
                };
                reporteList.Add(positivosSemPCR);

                var rcPositivosSemTAC = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSemTAC)
                    .Select(group => group.Sum(item => item.PositivosSemTAC))
                    //.OrderBy(n => n.PositivosSemTAC)
                    .ToListAsync();

                var positivosSemTAC =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales TAC",
                    TipoId = "rcPositivosSemTAC",
                    Cantidad = rcPositivosSemTAC.Sum()
                };
                reporteList.Add(positivosSemTAC);

                var rcPositivosSospechososNeuNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSospechososNeumoniaNoConfirmadaCOVID)
                    .Select(group => group.Sum(item => item.PositivosSospechososNeumoniaNoConfirmadaCOVID))
                    //.OrderBy(n => n.PositivosSospechososNeuNoConfCOVID)
                    .ToListAsync();

                var positivosSospechososNeuNoConfCOVID =
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Sospechosos Neumonia no confirmada Covid",
                    TipoId = "rcPositivosSospechososNeuNoConfCOVID",
                    Cantidad = rcPositivosSospechososNeuNoConfCOVID.Sum()
                };
                reporteList.Add(positivosSospechososNeuNoConfCOVID);
            }
            catch (Exception ex) 
            { }

            return reporteList;
        }

        public async Task<IActionResult> PositivosSospechososSemanalesXArea(int IdEmpresa)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdEmpresa;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var reporteContagios = await ObtenerReporteContagioSemanalXArea(idEmpresa);

            builder.AppendLine("Empresa,Area,Tipo,Cantidad");

            foreach (var item in reporteContagios)
            {
                builder.AppendLine($"{item.NombreEmpresa},{item.NombreArea},{item.Tipo},{item.Cantidad}");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptContagioPositivosSospAreaSemanal_", dateTime, ".csv"));
        }

        public async Task<List<ReporteContagioViewModel>> ObtenerReporteContagioSemanalXArea(int IdEmpresa)
        {
            var reporteList = new List<ReporteContagioViewModel>();

            try
            {
                var nombreEmpresa = _context.Empresas.Where(x => x.IdEmpresa == IdEmpresa).Select(y => y.Nombre).FirstOrDefault();
                var areas = _context.Areas.Where(a => a.IdEmpresa == IdEmpresa).ToList();

                var fechaActual = DateTime.Now.Date;
                var lunesSemanaPasada = fechaActual.AddDays(-(int)fechaActual.DayOfWeek - 6 - 7);
                var fechaFinal = lunesSemanaPasada.AddDays(6);

                var rcPosSemanalesAntigeno = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemAntigeno = rc.Key.PositivosSemAntigeno,
                        Counter = rc.Sum(b => b.PositivosSemAntigeno)
                    })
                    //.OrderBy(n => n.IdArea)
                    .ToListAsync();

                var posSemAntigeno = rcPosSemanalesAntigeno.Select(rpt => 
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    TipoId = "rcPosSemanalesAntigeno",
                    Tipo = "Positivos Semanales Antigeno",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemAntigeno);

                var rcPosSemanalesLG = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemLG = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemLG)
                    })
                    //.OrderBy(n => n.PosSemLG)
                    .ToListAsync();

                var posSemanalesLG = rcPosSemanalesLG.Select(rpt => 
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales LG",
                    TipoId = "rcPosSemanalesLG",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemanalesLG);

                var rcPosSemNeumoniaNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemNeumoniaNoConfCOVID = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemNeumoniaNoConfirmadaCOVID)
                    })
                    //.OrderBy(n => n.PosSemNeumoniaNoConfCOVID)
                    .ToListAsync();

                var posSemNeumoniaNoConfCOVID = rcPosSemNeumoniaNoConfCOVID.Select(rpt => 
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales Neumonia No confirmada Covid",
                    TipoId = "rcPosSemNeumoniaNoConfCOVID",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemNeumoniaNoConfCOVID);

                var rcPositivosSemPCR = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PositivosSemPCR = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemPCR)
                    })
                    //.OrderBy(n => n.PositivosSemPCR)
                    .ToListAsync();

                var positivosSemPCR = rcPositivosSemPCR.Select(rpt => 
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales PCR",
                    TipoId = "rcPositivosSemPCR",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSemPCR);

                var rcPositivosSemTAC = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea } )
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PositivosSemTAC = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemTAC)
                    })
                    //.OrderBy(n => n.PositivosSemTAC)
                    .ToListAsync();

                var positivosSemTAC = rcPositivosSemTAC.Select(rpt => 
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales TAC",
                    TipoId = "rcPositivosSemTAC",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSemTAC);

                var rcPositivosSospechososNeuNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea } )
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PositivosSospechososNeuNoConfCOVID = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSospechososNeumoniaNoConfirmadaCOVID)
                    })
                    //.OrderBy(n => n.PositivosSospechososNeuNoConfCOVID)
                    .ToListAsync();

                var positivosSospechososNeuNoConfCOVID = rcPositivosSospechososNeuNoConfCOVID.Select(rpt => 
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Sospechosos Neumonia no confirmada Covid",
                    TipoId = "rcPositivosSospechososNeuNoConfCOVID",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSospechososNeuNoConfCOVID);
            }
            catch (Exception ex)
            { }

            return reporteList;
        }

        public async Task<IActionResult> PositivosSemanalXArea(int IdEmpresa)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdEmpresa;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var reporteContagios = await ObtenerPositivosSemanalXArea(idEmpresa);

            builder.AppendLine("Empresa,Area,Tipo,Cantidad");

            foreach (var item in reporteContagios)
            {
                builder.AppendLine($"{item.NombreEmpresa},{item.NombreArea},{item.Tipo},{item.Cantidad}");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptContagioPositivosAreaSemanal_", dateTime, ".csv"));
        }

        public async Task<List<ReporteContagioViewModel>> ObtenerPositivosSemanalXArea(int IdEmpresa)
        {
            var reporteList = new List<ReporteContagioViewModel>();

            try
            {
                var nombreEmpresa = _context.Empresas.Where(x => x.IdEmpresa == IdEmpresa).Select(y => y.Nombre).FirstOrDefault();
                var areas = _context.Areas.Where(a => a.IdEmpresa == IdEmpresa).ToList();

                var fechaActual = DateTime.Now.Date;
                var lunesSemanaPasada = fechaActual.AddDays(-(int)fechaActual.DayOfWeek - 6);
                var fechaFinal = lunesSemanaPasada.AddDays(7);

                var rcPosSemanalesAntigeno = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemAntigeno = rc.Key.PositivosSemAntigeno,
                        Counter = rc.Sum(b => b.PositivosSemAntigeno)
                    })
                    //.OrderBy(n => n.PosSemAntigeno)
                    .ToListAsync();

                var posSemAntigeno = rcPosSemanalesAntigeno.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    TipoId = "rcPosSemanalesAntigeno",
                    Tipo = "Positivos Semanales Antigeno",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemAntigeno);

                var rcPosSemanalesLG = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemLG = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemLG)
                    })
                    //.OrderBy(n => n.PosSemLG)
                    .ToListAsync();

                var posSemanalesLG = rcPosSemanalesLG.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales LG",
                    TipoId = "rcPosSemanalesLG",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemanalesLG);

                var rcPosSemNeumoniaNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemNeumoniaNoConfCOVID = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemNeumoniaNoConfirmadaCOVID)
                    })
                    //.OrderBy(n => n.PositivosSemNeumoniaNoConfirmadaCOVID)
                    .ToListAsync();

                var posSemNeumoniaNoConfCOVID = rcPosSemNeumoniaNoConfCOVID.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales Neumonia No confirmada Covid",
                    TipoId = "rcPosSemNeumoniaNoConfCOVID",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemNeumoniaNoConfCOVID);

                var rcPositivosSemPCR = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PositivosSemPCR = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemPCR)
                    })
                    //.OrderBy(n => n.PositivosSemPCR)
                    .ToListAsync();

                var positivosSemPCR = rcPositivosSemPCR.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales PCR",
                    TipoId = "rcPositivosSemPCR",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSemPCR);

                var rcPositivosSemTAC = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => lunesSemanaPasada <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PositivosSemTAC = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemTAC)
                    })
                    //.OrderBy(n => n.PositivosSemTAC)
                    .ToListAsync();

                var positivosSemTAC = rcPositivosSemTAC.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales TAC",
                    TipoId = "rcPositivosSemTAC",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSemTAC);
            }
            catch (Exception ex)
            { }

            return reporteList;
        }

        public async Task<IActionResult> PositivosMensualXArea(int IdEmpresa)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdEmpresa;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var reporteContagios = await ObtenerPositivosMensualXArea(idEmpresa);

            builder.AppendLine("Empresa,Area,Tipo,Cantidad");

            foreach (var item in reporteContagios)
            {
                builder.AppendLine($"{item.NombreEmpresa},{item.NombreArea},{item.Tipo},{item.Cantidad}");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptContagioPositivosAreaMensual_", dateTime, ".csv"));
        }

        public async Task<List<ReporteContagioViewModel>> ObtenerPositivosMensualXArea(int IdEmpresa)
        {
            var reporteList = new List<ReporteContagioViewModel>();

            try
            {
                var nombreEmpresa = _context.Empresas.Where(x => x.IdEmpresa == IdEmpresa).Select(y => y.Nombre).FirstOrDefault();
                var areas = _context.Areas.Where(a => a.IdEmpresa == IdEmpresa).ToList();

                var fechaRef = DateTime.Now.AddMonths(-1);
                var anio = fechaRef.Year;
                var month = fechaRef.Month;
                var fechaInicio = new DateTime(anio, month, 1);
                var ultimoDiaMes = DateTime.DaysInMonth(anio, month);
                var fechaFinal = new DateTime(anio, month, ultimoDiaMes);

                var rcPosSemanalesAntigeno = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemAntigeno = rc.Key.PositivosSemAntigeno,
                        Counter = rc.Sum(b => b.PositivosSemAntigeno)
                    })
                    //.OrderBy(n => n.PosSemAntigeno)
                    .ToListAsync();

                var posSemAntigeno = rcPosSemanalesAntigeno.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    TipoId = "rcPosSemanalesAntigeno",
                    Tipo = "Positivos Semanales Antigeno",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemAntigeno);

                var rcPosSemanalesLG = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemLG = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemLG)
                    })
                    //.OrderBy(n => n.PosSemLG)
                    .ToListAsync();

                var posSemanalesLG = rcPosSemanalesLG.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales LG",
                    TipoId = "rcPosSemanalesLG",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemanalesLG);

                var rcPosSemNeumoniaNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemNeumoniaNoConfCOVID = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemNeumoniaNoConfirmadaCOVID)
                    })
                    //.OrderBy(n => n.PosSemNeumoniaNoConfCOVID)
                    .ToListAsync();

                var posSemNeumoniaNoConfCOVID = rcPosSemNeumoniaNoConfCOVID.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales Neumonia No confirmada Covid",
                    TipoId = "rcPosSemNeumoniaNoConfCOVID",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemNeumoniaNoConfCOVID);

                var rcPositivosSemPCR = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PositivosSemPCR = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemPCR)
                    })
                    //.OrderBy(n => n.PositivosSemPCR)
                    .ToListAsync();

                var positivosSemPCR = rcPositivosSemPCR.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales PCR",
                    TipoId = "rcPositivosSemPCR",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSemPCR);

                var rcPositivosSemTAC = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PositivosSemTAC = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemTAC)
                    })
                    //.OrderBy(n => n.PositivosSemTAC)
                    .ToListAsync();

                var positivosSemTAC = rcPositivosSemTAC.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales TAC",
                    TipoId = "rcPositivosSemTAC",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSemTAC);


                var rcPositivosSospechososNeuNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        Counter = rc.Sum(b => b.PositivosSospechososNeumoniaNoConfirmadaCOVID)
                    })
                    .ToListAsync();

                var positivosSospechososNeuNoConfCOVID = rcPositivosSospechososNeuNoConfCOVID.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Sospechosos Neumonia no confirmada Covid",
                    TipoId = "rcPositivosSospechososNeuNoConfCOVID",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSospechososNeuNoConfCOVID);
            }
            catch (Exception ex)
            { }

            return reporteList;
        }

        public async Task<IActionResult> PositivosTotalXArea(int IdEmpresa)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdEmpresa;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var reporteContagios = await ObtenerPositivosTotalXArea(idEmpresa);

            builder.AppendLine("Empresa,Area,Tipo,Cantidad");

            foreach (var item in reporteContagios)
            {
                builder.AppendLine($"{item.NombreEmpresa},{item.NombreArea},{item.Tipo},{item.Cantidad}");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptContagioPositivosAreaTotal_", dateTime, ".csv"));
        }

        public async Task<List<ReporteContagioViewModel>> ObtenerPositivosTotalXArea(int IdEmpresa)
        {
            var reporteList = new List<ReporteContagioViewModel>();

            try
            {
                var nombreEmpresa = _context.Empresas.Where(x => x.IdEmpresa == IdEmpresa).Select(y => y.Nombre).FirstOrDefault();
                var areas = _context.Areas.Where(a => a.IdEmpresa == IdEmpresa).ToList();

                var rcPosSemanalesAntigeno = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemAntigeno = rc.Key.PositivosSemAntigeno,
                        Counter = rc.Sum(b => b.PositivosSemAntigeno)
                    })
                    //.OrderBy(n => n.PosSemAntigeno)
                    .ToListAsync();

                var posSemAntigeno = rcPosSemanalesAntigeno.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    TipoId = "rcPosSemanalesAntigeno",
                    Tipo = "Positivos Semanales Antigeno",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemAntigeno);

                var rcPosSemanalesLG = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemLG = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemLG)
                    })
                    //.OrderBy(n => n.PosSemLG)
                    .ToListAsync();

                var posSemanalesLG = rcPosSemanalesLG.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales LG",
                    TipoId = "rcPosSemanalesLG",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemanalesLG);

                var rcPosSemNeumoniaNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PosSemNeumoniaNoConfCOVID = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemNeumoniaNoConfirmadaCOVID)
                    })
                    //.OrderBy(n => n.PosSemNeumoniaNoConfCOVID)
                    .ToListAsync();

                var posSemNeumoniaNoConfCOVID = rcPosSemNeumoniaNoConfCOVID.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales Neumonia No confirmada Covid",
                    TipoId = "rcPosSemNeumoniaNoConfCOVID",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(posSemNeumoniaNoConfCOVID);

                var rcPositivosSemPCR = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PositivosSemPCR = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemPCR)
                    })
                    //.OrderBy(n => n.PositivosSemPCR)
                    .ToListAsync();

                var positivosSemPCR = rcPositivosSemPCR.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales PCR",
                    TipoId = "rcPositivosSemPCR",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSemPCR);

                var rcPositivosSemTAC = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PositivosSemTAC = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSemTAC)
                    })
                    //.OrderBy(n => n.PositivosSemTAC)
                    .ToListAsync();

                var positivosSemTAC = rcPositivosSemTAC.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Semanales TAC",
                    TipoId = "rcPositivosSemTAC",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSemTAC);

                var rcPositivosSosNeuNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => new { rc.IdArea })
                    .Select(rc => new
                    {
                        IdArea = rc.Key.IdArea,
                        //PositivosSemTAC = rc.Key,
                        Counter = rc.Sum(b => b.PositivosSospechososNeumoniaNoConfirmadaCOVID)
                    })
                    //.OrderBy(n => n.PositivosSemTAC)
                    .ToListAsync();

                var positivosSosNeuNoConfCOVID = rcPositivosSosNeuNoConfCOVID.Select(rpt =>
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    NombreArea = areas.Where(x => x.Id == rpt.IdArea).Select(y => y.Name).FirstOrDefault(),
                    Tipo = "Positivos Sospechosos Neumonia no confirmada Covid",
                    TipoId = "rcPositivosSospechososNeuNoConfCOVID",
                    Cantidad = rpt.Counter
                }).ToList();
                reporteList.AddRange(positivosSosNeuNoConfCOVID);
            }
            catch (Exception ex)
            { }

            return reporteList;
        }

        public async Task<IActionResult> PositivosSospechososMensuales(int IdEmpresa)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdEmpresa;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var reporteContagios = await ObtenerReporteContagioMensuales(idEmpresa);

            builder.AppendLine("Empresa,Tipo,Cantidad");

            foreach (var item in reporteContagios)
            {
                builder.AppendLine($"{item.NombreEmpresa},{item.Tipo},{item.Cantidad}");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptContagioPositivosSospMensual_", dateTime, ".csv"));
        }

        public async Task<List<ReporteContagioViewModel>> ObtenerReporteContagioMensuales(int IdEmpresa)
        {
            var reporteList = new List<ReporteContagioViewModel>();

            try
            {
                var nombreEmpresa = _context.Empresas.Where(x => x.IdEmpresa == IdEmpresa).Select(y => y.Nombre).FirstOrDefault();

                var fechaRef = DateTime.Now.AddMonths(-1);
                var anio = fechaRef.Year;
                var month = fechaRef.Month;
                var fechaInicio = new DateTime(anio, month, 1);
                var ultimoDiaMes = DateTime.DaysInMonth(anio, month);
                var fechaFinal = new DateTime(anio, month, ultimoDiaMes);

                 var rcPosSemanalesAntigeno = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSemAntigeno)
                    .Select(group => group.Sum(item => item.PositivosSemAntigeno))
                    //.OrderBy(n => n.PosSemAntigeno)
                    .ToListAsync();

                var posSemAntigeno = 
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales Antigeno",
                    TipoId = "rcPosSemanalesAntigeno",
                    Cantidad = rcPosSemanalesAntigeno.Sum()
                };
                reporteList.Add(posSemAntigeno);

                var rcPosSemanalesLG = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSemLG)
                    .Select(group => group.Sum(item => item.PositivosSemLG))
                    //.OrderBy(n => n.PosSemLG)
                    .ToListAsync();

                var posSemanalesLG = 
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales LG",
                    TipoId = "rcPosSemanalesLG",
                    Cantidad = rcPosSemanalesLG.Sum()
                };
                reporteList.Add(posSemanalesLG);

                var rcPosSemNeumoniaNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSemNeumoniaNoConfirmadaCOVID)
                    .Select(group => group.Sum(item => item.PositivosSemNeumoniaNoConfirmadaCOVID))
                    //.OrderBy(n => n.PosSemNeumoniaNoConfCOVID)
                    .ToListAsync();

                var posSemNeumoniaNoConfCOVID =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales Neumonia No confirmada Covid",
                    TipoId = "rcPosSemNeumoniaNoConfCOVID",
                    Cantidad = rcPosSemNeumoniaNoConfCOVID.Sum()
                };
                reporteList.Add(posSemNeumoniaNoConfCOVID);

                var rcPositivosSemPCR = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSemPCR)
                    .Select(group => group.Sum(item => item.PositivosSemPCR))
                    //.OrderBy(n => n.PositivosSemPCR)
                    .ToListAsync();

                var positivosSemPCR =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales PCR",
                    TipoId = "rcPositivosSemPCR",
                    Cantidad = rcPositivosSemPCR.Sum()
                };
                reporteList.Add(positivosSemPCR);

                var rcPositivosSemTAC = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSemTAC)
                    .Select(group => group.Sum(item => item.PositivosSemTAC))
                    //.OrderBy(n => n.PositivosSemTAC)
                    .ToListAsync();

                var positivosSemTAC =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales TAC",
                    TipoId = "rcPositivosSemTAC",
                    Cantidad = rcPositivosSemTAC.Sum()
                };
                reporteList.Add(positivosSemTAC);

                var rcPositivosSospechososNeuNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .Where(rep => fechaInicio <= rep.FechaRegistro && fechaFinal >= rep.FechaRegistro)
                    .GroupBy(rc => rc.PositivosSospechososNeumoniaNoConfirmadaCOVID)
                    .Select(group => group.Sum(item => item.PositivosSospechososNeumoniaNoConfirmadaCOVID))
                    //.OrderBy(n => n.PositivosSospechososNeuNoConfCOVID)
                    .ToListAsync();

                var positivosSospechososNeuNoConfCOVID = 
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Sospechosos Neumonia no confirmada Covid",
                    TipoId = "rcPositivosSospechososNeuNoConfCOVID",
                    Cantidad = rcPositivosSospechososNeuNoConfCOVID.Sum()
                };
                reporteList.Add(positivosSospechososNeuNoConfCOVID);
            }
            catch (Exception ex)
            { }

            return reporteList;
        }

        public async Task<IActionResult> PositivosSospechososEmpresa(int IdEmpresa)
        {
            var builder = new StringBuilder();

            int idEmpresa = IdEmpresa;
            if (idEmpresa == 0)
            {
                return RedirectToAction(nameof(PreIndex));
            }

            var reporteContagios = await ObtenerReporteContagioEmpresa(idEmpresa);

            builder.AppendLine("Empresa,Tipo,Cantidad");

            foreach (var item in reporteContagios)
            {
                builder.AppendLine($"{item.NombreEmpresa},{item.Tipo},{item.Cantidad}");
            }
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", string.Concat("rptContagioPositivosSospEmpresa_", dateTime, ".csv"));
        }

        public async Task<List<ReporteContagioViewModel>> ObtenerReporteContagioEmpresa(int IdEmpresa)
        {
            var reporteList = new List<ReporteContagioViewModel>();

            try
            {
                var nombreEmpresa = _context.Empresas.Where(x => x.IdEmpresa == IdEmpresa).Select(y => y.Nombre).FirstOrDefault();

                var rcPosSemanalesAntigeno = await _context.ReporteContagio
                   .Where(emp => emp.IdEmpresa == IdEmpresa)
                   .GroupBy(rc => rc.PositivosSemAntigeno)
                   .Select(group => group.Sum(item => item.PositivosSemAntigeno))
                   //.OrderBy(n => n.PosSemAntigeno)
                   .ToListAsync();

                var posSemAntigeno =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales Antigeno",
                    TipoId = "rcPosSemanalesAntigeno",
                    Cantidad = rcPosSemanalesAntigeno.Sum()
                };
                reporteList.Add(posSemAntigeno);

                var rcPosSemanalesLG = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => rc.PositivosSemLG)
                    .Select(group => group.Sum(item => item.PositivosSemLG))
                    //.OrderBy(n => n.PosSemLG)
                    .ToListAsync();

                var posSemanalesLG = 
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales LG",
                    TipoId = "rcPosSemanalesLG",
                    Cantidad = rcPosSemanalesLG.Sum()
                };
                reporteList.Add(posSemanalesLG);

                var rcPosSemNeumoniaNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => rc.PositivosSemNeumoniaNoConfirmadaCOVID)
                    .Select(group => group.Sum(item => item.PositivosSemNeumoniaNoConfirmadaCOVID))
                    //.OrderBy(n => n.PosSemNeumoniaNoConfCOVID)
                    .ToListAsync();

                var posSemNeumoniaNoConfCOVID =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales Neumonia No confirmada Covid",
                    TipoId = "rcPosSemNeumoniaNoConfCOVID",
                    Cantidad = rcPosSemNeumoniaNoConfCOVID.Sum()
                };
                reporteList.Add(posSemNeumoniaNoConfCOVID);

                var rcPositivosSemPCR = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => rc.PositivosSemPCR)
                    .Select(group => group.Sum(item => item.PositivosSemPCR))
                    //.OrderBy(n => n.PositivosSemPCR)
                    .ToListAsync();

                var positivosSemPCR =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales PCR",
                    TipoId = "rcPositivosSemPCR",
                    Cantidad = rcPositivosSemPCR.Sum()
                };
                reporteList.Add(positivosSemPCR);

                var rcPositivosSemTAC = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => rc.PositivosSemTAC)
                    .Select(group => group.Sum(item => item.PositivosSemTAC))
                    //.OrderBy(n => n.PositivosSemTAC)
                    .ToListAsync();

                var positivosSemTAC =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Semanales TAC",
                    TipoId = "rcPositivosSemTAC",
                    Cantidad = rcPositivosSemTAC.Sum()
                };
                reporteList.Add(positivosSemTAC);

                var rcPositivosSospechososNeuNoConfCOVID = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => rc.PositivosSospechososNeumoniaNoConfirmadaCOVID)
                    .Select(group => group.Sum(item => item.PositivosSospechososNeumoniaNoConfirmadaCOVID))
                    //.OrderBy(n => n.PositivosSospechososNeuNoConfCOVID)
                    .ToListAsync();

                var positivosSospechososNeuNoConfCOVID =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Positivos Sospechosos Neumonia no confirmada Covid",
                    TipoId = "rcPositivosSospechososNeuNoConfCOVID",
                    Cantidad = rcPositivosSospechososNeuNoConfCOVID.Sum()
                };
                reporteList.Add(positivosSospechososNeuNoConfCOVID);

                var rcSospechososDescartados = await _context.ReporteContagio
                    .Where(emp => emp.IdEmpresa == IdEmpresa)
                    .GroupBy(rc => rc.SospechososDescartados)
                    .Select(group => group.Sum(item => item.SospechososDescartados))
                    //.OrderBy(n => n.SospechososDescartados)
                    .ToListAsync();

                var sospechososDescartados =  
                new ReporteContagioViewModel
                {
                    NombreEmpresa = nombreEmpresa,
                    Tipo = "Sospechosos Descartados",
                    TipoId = "rcSospechososDescartados",
                    Cantidad = rcSospechososDescartados.Sum()
                };
                reporteList.Add(sospechososDescartados);

            }
            catch (Exception ex)
            { }

            return reporteList;
        }
    }
}
