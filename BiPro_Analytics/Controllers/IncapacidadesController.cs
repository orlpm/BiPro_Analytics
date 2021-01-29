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

namespace BiPro_Analytics.Controllers
{
    public class IncapacidadesController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public IncapacidadesController(BiproAnalyticsDBContext context)
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

                unidades = await _context.Unidades.Where(u => u.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                ViewBag.Unidades = unidades;
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
                    ViewBag.Trabajadores = trabajadores;
                }
                else
                {
                    return NotFound("Usuario no vinculado a trabajador");
                }
            }

            return View();
        }
        // GET: Incapacidades
        public async Task<IActionResult> Index(int? IdTrabajador, int? IdUnidad, int? IdArea)
        {
            ClaimsPrincipal currentUser = this.User;
            Empresa empresa = null;
            UsuarioTrabajador usuarioTrabajador = null;

            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUserId == null)
                return NotFound();

            usuarioTrabajador = await _context.UsuariosTrabajadores
                .FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));
            empresa = await _context.Empresas
                .FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);

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
                List<Trabajador> trabajadores;

                if (IdTrabajador != null)
                {
                    return View(await _context.Incapacidades
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == empresa.IdEmpresa).ToListAsync();

                    if (trabajadores.Count > 0)
                    {
                        List<Incapacidad> registroPruebas = new List<Incapacidad>();

                        foreach (var trabajador in trabajadores)
                        {
                            var incapacidades = await _context.Incapacidades
                                .FirstOrDefaultAsync(x => x.IdTrabajador == trabajador.IdTrabajador);

                            if (incapacidades != null)
                                registroPruebas.Add(incapacidades);
                        }

                        if (registroPruebas.Count > 0)
                            return View(registroPruebas);
                        else
                        {
                            return NotFound("no mames no está lo que buscas morro");
                        }
                    }
                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .Where(q => q != null && q.IdArea == IdArea)
                        .SelectMany(r => r.Incapacidades)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var vpruebas = await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .ToListAsync();

                    var pruebas = await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.Incapacidades)
                        .ToListAsync();

                    return View(pruebas);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.Incapacidades)
                        .ToListAsync());
                }
            }
            else
            {
                var registroPruebas = await _context.Incapacidades
                    .Where(r => r.IdTrabajador == usuarioTrabajador.TrabajadorId).ToListAsync();

                if (registroPruebas.Count == 0)
                    return NotFound("Sin registros de pruebas");

                return View(registroPruebas);
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
        public IActionResult Create()
        {
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
