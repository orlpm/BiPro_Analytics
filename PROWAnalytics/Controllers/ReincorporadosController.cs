using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROWAnalytics.Data;
using PROWAnalytics.Models;
using System.Security.Claims;
using PROWAnalytics.UnParo;
using PROWAnalytics.Responses;

namespace PROWAnalytics.Controllers
{
    public class ReincorporadosController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public ReincorporadosController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
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

        // GET: Reincorporados
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
                    return View(await _context.Reincorporados
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .Include(x=>x.Trabajador)
                        .ToListAsync());

                if (IdUnidad != null && IdArea == null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad)
                        .SelectMany(r => r.Reincorporados)
                        .Include(x => x.Trabajador)
                        .ToListAsync());

                if (IdUnidad == null && IdArea != null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdArea == IdArea)
                        .SelectMany(r => r.Reincorporados)
                        .Include(x => x.Trabajador)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea)
                        .SelectMany(r => r.Reincorporados)
                        .Include(x => x.Trabajador)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (IdTrabajador != null)
                {
                    return View(await _context.Reincorporados
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .Include(x => x.Trabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {

                    List<Reincorporado> Reincorporados = new List<Reincorporado>();

                    Reincorporados = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(t => t.Reincorporados)
                        .Include(x => x.Trabajador)
                        .ToListAsync();

                    return View(Reincorporados);

                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.Reincorporados)
                        .Include(x => x.Trabajador)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var riesgos = await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.Reincorporados)
                        .Include(x => x.Trabajador)
                        .ToListAsync();

                    return View(riesgos);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdArea == IdArea && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.Reincorporados)
                        .Include(x => x.Trabajador)
                        .ToListAsync());
                }
            }
            else
            {
                var reicorporados = await _context.Reincorporados
                    .Where(r => r.IdTrabajador == perfilData.IdTrabajador)
                    .Include(x => x.Trabajador)
                    .ToListAsync();

                if (reicorporados.Count == 0)
                    return NotFound();

                return View(reicorporados);
            }

            return View(await _context.Reincorporados
                .Include(x => x.Trabajador)
                .ToListAsync());
        }

        // GET: Reincorporados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reincorporados = await _context.Reincorporados
                .Include(x => x.Trabajador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reincorporados == null)
            {
                return NotFound();
            }

            return View(reincorporados);
        }

        // GET: Reincorporados/Create
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            return View();
        }

        // POST: Reincorporados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaDiagnostico,FechaTerminoIncapacidad,FechaRegresoTrabajo,DiasIncapacidad,EtudiosSecuelasPulmonares,EtudiosSecuelasNoPulmonares,FisicamenteCapacitado,MotivadoTrabajo,MedicoSeguimiento,IdTrabajador")] Reincorporado reincorporados)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (ModelState.IsValid)
            {
                if (currentUser.IsInRole("Trabajador"))
                    reincorporados.Trabajador = _context.Trabajadores.Find(perfilData.IdTrabajador);
                else
                    reincorporados.Trabajador = _context.Trabajadores.Find(reincorporados.IdTrabajador);

                reincorporados.FechaRegistro = DateTime.Now;

                _context.Add(reincorporados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reincorporados);
        }

        // GET: Reincorporados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            var reincorporados = await _context.Reincorporados.FindAsync(id);

            if (reincorporados == null)
            {
                return NotFound();
            }
            return View(reincorporados);
        }

        // POST: Reincorporados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaDiagnostico,FechaTerminoIncapacidad,FechaRegresoTrabajo,DiasIncapacidad,EtudiosSecuelasPulmonares,EtudiosSecuelasNoPulmonares,FisicamenteCapacitado,MotivadoTrabajo,MedicoSeguimiento,IdTrabajador")] Reincorporado reincorporados)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (id != reincorporados.Id)
            {
                return NotFound();
            }

            if (currentUser.IsInRole("Trabajador"))
                reincorporados.Trabajador = _context.Trabajadores.Find(perfilData.IdTrabajador);
            else
                reincorporados.Trabajador = _context.Trabajadores.Find(reincorporados.IdTrabajador);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reincorporados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReincorporadosExists(reincorporados.Id))
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
            return View(reincorporados);
        }

        // GET: Reincorporados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reincorporados = await _context.Reincorporados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reincorporados == null)
            {
                return NotFound();
            }

            return View(reincorporados);
        }

        // POST: Reincorporados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reincorporados = await _context.Reincorporados.FindAsync(id);
            _context.Reincorporados.Remove(reincorporados);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReincorporadosExists(int id)
        {
            return _context.Reincorporados.Any(e => e.Id == id);
        }
    }
}
