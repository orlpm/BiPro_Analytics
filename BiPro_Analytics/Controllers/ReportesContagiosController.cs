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
using BiPro_Analytics.UnParo;
using BiPro_Analytics.Responses;
using Microsoft.AspNetCore.Authorization;

namespace BiPro_Analytics.Controllers
{
    [Authorize(Roles = "Admin,AdminEmpresa")]
    public class ReportesContagiosController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public ReportesContagiosController(BiproAnalyticsDBContext context)
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
                    return NotFound("Usuario no asociado a ninguna empresa");
            }
            ViewBag.IdEmpresa = perfilData.IdEmpresa;

            return View();
        }

        // GET: ReportesContagios
        public async Task<IActionResult> Index(int? IdEmpresa)
        {
            ClaimsPrincipal currentUser = this.User;
            UsuarioTrabajador usuarioTrabajador = null;
            UsuarioEmpresa usuarioEmpresa = null;
            Empresa empresa = null;

            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserId != null)
            {
                usuarioTrabajador = await _context.UsuariosTrabajadores.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));
                usuarioEmpresa = await _context.UsuariosEmpresas.FirstOrDefaultAsync(u => u.IdUsuario == Guid.Parse(currentUserId));
            }

            if (usuarioTrabajador != null)
                empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);

            if (currentUser.IsInRole("Admin"))
            {
                if (IdEmpresa != null)
                    return View(await _context.ReporteContagio
                        .Where(x => x.IdEmpresa == IdEmpresa).Include(x=>x.Empresa).Include(x => x.Unidad).Include(x => x.Area)
                        .ToListAsync());
                else
                    return View(await _context.ReporteContagio.Include(x => x.Empresa).Include(x => x.Unidad).Include(x => x.Area)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (usuarioEmpresa != null)
                {
                    return View(await _context.ReporteContagio
                        .Where(x => x.IdEmpresa == usuarioEmpresa.IdEmpresa).Include(x => x.Empresa).Include(x => x.Unidad).Include(x => x.Area)
                        .ToListAsync());
                }
                else
                {
                    return NotFound();
                }
            }

            return View(await _context.ReporteContagio.Include(x => x.Empresa).Include(x => x.Unidad).Include(x => x.Area)
                .ToListAsync());
        }

        // GET: ReportesContagios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reporteContagio = await _context.ReporteContagio
                .Include(r => r.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reporteContagio == null)
            {
                return NotFound();
            }

            return View(reporteContagio);
        }

        // GET: ReportesContagios/Create
        public IActionResult Create()
        {
            ViewData["TiposDePrueba"] = new SelectList(_context.TiposPruebas, "Id", "TipoDePrueba");

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");
            ViewData["Unidades"] = new SelectList(_context.Unidades, "Id", "Nombre");
            ViewData["Areas"] = new SelectList(_context.Areas, "Id", "Nombre");
            return View();
        }

        // POST: ReportesContagios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoPrueba,NumeroPruebas,Positivos,Negativos,FechaInicio,FechaFin,FechaRegistro,IdEmpresa,IdUnidad,IdArea")] ReporteContagio reporteContagio)
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (User.IsInRole("AdminEmpresa"))
                reporteContagio.Empresa = await _context.Empresas.FindAsync(perfilData.IdEmpresa);

            reporteContagio.FechaRegistro = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(reporteContagio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (User.IsInRole("AdminEmpresa"))
                ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre", perfilData.IdEmpresa);
            else
                ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre", reporteContagio.IdEmpresa);

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
            
            ViewData["TiposDePrueba"] = new SelectList(_context.TiposPruebas, "Id", "TipoDePrueba");

            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre");
            ViewData["Unidades"] = new SelectList(_context.Unidades, "Id", "Nombre");
            ViewData["Areas"] = new SelectList(_context.Areas, "Id", "Nombre");

            return View(reporteContagio);
        }

        // POST: ReportesContagios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TipoPrueba,NumeroPruebas,Positivos,Negativos,FechaInicio,FechaFin,FechaRegistro,IdEmpresa,IdUnidad,IdArea")] ReporteContagio reporteContagio)
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (id != reporteContagio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (User.IsInRole("AdminEmpresa"))
                        reporteContagio.Empresa = await _context.Empresas.FindAsync(perfilData.IdEmpresa);

                    reporteContagio.FechaRegistro = DateTime.Now;
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
            ViewData["IdEmpresa"] = new SelectList(_context.Empresas, "IdEmpresa", "Nombre", reporteContagio.IdEmpresa);
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
                .Include(r => r.Empresa)
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
