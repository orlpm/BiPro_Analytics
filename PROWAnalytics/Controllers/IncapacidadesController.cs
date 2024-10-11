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
using PROWAnalytics.Responses;
using PROWAnalytics.UnParo;

namespace PROWAnalytics.Controllers
{
    public class IncapacidadesController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public IncapacidadesController(BiproAnalyticsDBContext context)
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
        // GET: Incapacidades
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
                    return View(await _context.Incapacidades
                        .Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdUnidad != null)
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.Incapacidades)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.Incapacidades)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {

                if (IdTrabajador != null)
                {
                    return View(await _context.Incapacidades
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {

                    List<Incapacidad> incapacidades = new List<Incapacidad>();

                    incapacidades = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(t => t.Incapacidades).ToListAsync();

                    return View(incapacidades);

                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea)
                        .SelectMany(r => r.Incapacidades)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var incapacidades = await _context.Trabajadores
                        .Where(u => u.IdUnidad == IdUnidad)
                        .SelectMany(r => r.Incapacidades)
                        .ToListAsync();

                    return View(incapacidades);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(a => a.IdArea == IdArea)
                        .SelectMany(r => r.Incapacidades)
                        .ToListAsync());
                }
            }
            else
            {
                var incapacidades = await _context.Incapacidades
                    .Where(r => r.IdTrabajador == perfilData.IdTrabajador).ToListAsync();

                if (incapacidades.Count == 0)
                    return NotFound("Trabajador sin Incapacidades");

                return View(incapacidades);
            }

            return View(await _context.Incapacidades.ToListAsync());

        }

        // GET: Incapacidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incapacidad = await _context.Incapacidades
                .FirstOrDefaultAsync(m => m.IdIncapacidad == id);
            if (incapacidad == null)
            {
                return NotFound();
            }

            return View(incapacidad);
        }

        // GET: Incapacidades/Create
        public async Task<IActionResult> Create()
        {
            //Para combo Trabajadores
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;
            ViewBag.Empresas = perfilData.DDLEmpresas;

            return View();
        }

        // POST: Incapacidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdIncapacidad,NombreEmpleado,FechaHoraInicio,FechaHoraFin,MotivoIncapacidad,TipoIncapacidad,SeEncuentraEn,IdTrabajador")] Incapacidad incapacidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incapacidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(incapacidad);
        }

        // GET: Incapacidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incapacidad = await _context.Incapacidades.FindAsync(id);
            if (incapacidad == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;
            ViewBag.Empresas = perfilData.DDLEmpresas;

            return View(incapacidad);
        }

        // POST: Incapacidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdIncapacidad,NombreEmpleado,FechaHoraInicio,FechaHoraFin,MotivoIncapacidad,TipoIncapacidad,SeEncuentraEn,IdTrabajador")] Incapacidad incapacidad)
        {
            if (id != incapacidad.IdIncapacidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incapacidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncapacidadExists(incapacidad.IdIncapacidad))
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
            return View(incapacidad);
        }

        // GET: Incapacidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incapacidad = await _context.Incapacidades
                .FirstOrDefaultAsync(m => m.IdIncapacidad == id);
            if (incapacidad == null)
            {
                return NotFound();
            }

            return View(incapacidad);
        }

        // POST: Incapacidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incapacidad = await _context.Incapacidades.FindAsync(id);
            _context.Incapacidades.Remove(incapacidad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncapacidadExists(int id)
        {
            return _context.Incapacidades.Any(e => e.IdIncapacidad == id);
        }
    }
}
