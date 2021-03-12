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
    public class PruebasController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public PruebasController(BiproAnalyticsDBContext context)
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
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                {
                    return NotFound("Datos de empresa no encontrados");
                }
            }

            return View();
        }
        // GET: Pruebas

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
                    return View(await _context.Pruebas
                        .Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdUnidad != null)
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.Pruebas)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.Pruebas)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {

                if (IdTrabajador != null)
                {
                    return View(await _context.Pruebas
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {

                    List<Prueba> pruebas = new List<Prueba>();

                    pruebas = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(t => t.Pruebas).ToListAsync();

                    return View(pruebas);

                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea)
                        .SelectMany(r => r.Pruebas)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var pruebas = await _context.Trabajadores
                        .Where(u => u.IdUnidad == IdUnidad)
                        .SelectMany(r => r.Pruebas)
                        .ToListAsync();

                    return View(pruebas);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(a => a.IdArea == IdArea)
                        .SelectMany(r => r.Pruebas)
                        .ToListAsync());
                }
            }
            else
            {
                var pruebas = await _context.Pruebas
                    .Where(r => r.IdTrabajador == perfilData.IdTrabajador).ToListAsync();

                if (pruebas.Count == 0)
                    return NotFound("Trabajador sin pruebas");

                return View(pruebas);
            }

            return View(await _context.Pruebas.ToListAsync());

        }

        // GET: Pruebas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prueba = await _context.Pruebas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prueba == null)
            {
                return NotFound();
            }

            return View(prueba);
        }

        // GET: Pruebas/Create
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;
            var ubicaciones = _context.Ubicacion.ToListAsync();
            ViewBag.Ubicaciones = new SelectList(ubicaciones.Result, "Identificador", "Descripcion");

            return View();
        }

        // POST: Pruebas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaDiagnostico,Lugar,TipoPrueba,DiagnosticoCovid,SintomasCovid,RadiografiaTorax,Tomografia,IdTrabajador,UbicacionId")] Prueba prueba)
        {
            if (ModelState.IsValid)
            {
                prueba.Trabajador = _context.Trabajadores.Find(prueba.IdTrabajador);
                prueba.FechaHoraRegistro = DateTime.Now;
                _context.Add(prueba);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prueba);
        }

        // GET: Pruebas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prueba = await _context.Pruebas.FindAsync(id);
            if (prueba == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            return View(prueba);
        }

        // POST: Pruebas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaDiagnostico,Lugar,TipoPrueba,DiagnosticoCovid,SintomasCovid,RadiografiaTorax,Tomografia,IdTrabajador")] Prueba prueba)
        {
            if (id != prueba.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    prueba.Trabajador = _context.Trabajadores.Find(prueba.IdTrabajador);
                    prueba.FechaHoraRegistro = DateTime.Now;
                    _context.Update(prueba);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PruebaExists(prueba.Id))
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
            return View(prueba);
        }

        // GET: Pruebas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prueba = await _context.Pruebas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prueba == null)
            {
                return NotFound();
            }

            return View(prueba);
        }

        // POST: Pruebas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prueba = await _context.Pruebas.FindAsync(id);
            _context.Pruebas.Remove(prueba);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PruebaExists(int id)
        {
            return _context.Pruebas.Any(e => e.Id == id);
        }
    }
}
