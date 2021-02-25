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

                ViewBag.Unidades = unidades;
                ViewBag.areas = areas;
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

                    areas = await _context.Areas.Where(a => a.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                    ViewBag.Areas = areas;

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

                    ViewBag.Trabajadores = trabajadores;
                    ViewBag.Unidades = unidades;
                    ViewBag.areas = areas;

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

            return View();
        }

        // POST: ReportesContagios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Semana,Anio,FechaRegistro,PositivosSemPCR,PositivosSemLG,PositivosSemAntigeno,PositivosSemTAC,PositivosSemNeumoniaNoConfirmadaCOVID,PositivosSospechososNeumoniaNoConfirmadaCOVID,SospechososDescartados,IdEmpresa")] ReporteContagio reporteContagio)
        {
            if (ModelState.IsValid)
            {
                reporteContagio.Empresa = _context.Empresas.Find(reporteContagio.IdEmpresa);
                _context.Add(reporteContagio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
    }
}
