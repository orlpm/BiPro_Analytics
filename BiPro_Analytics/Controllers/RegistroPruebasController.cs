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
    public class RegistroPruebasController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public RegistroPruebasController(BiproAnalyticsDBContext context)
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
        // GET: RegistroPruebas
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
                    return View(await _context.RegistroPruebas
                        .Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdUnidad != null)
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.RegistroPruebas)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.RegistroPruebas)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                List<Trabajador> trabajadores;

                if (IdTrabajador != null)
                {
                    return View(await _context.RegistroPruebas
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == empresa.IdEmpresa).ToListAsync();

                    if (trabajadores.Count > 0)
                    {
                        List<RegistroPrueba> registroPruebas = new List<RegistroPrueba>();

                        foreach (var trabajador in trabajadores)
                        {
                            var registroPrueba = await _context.RegistroPruebas
                                .FirstOrDefaultAsync(x => x.IdTrabajador == trabajador.IdTrabajador);

                            if (registroPrueba != null)
                                registroPruebas.Add(registroPrueba);
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
                        .SelectMany(r => r.RegistroPruebas)
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
                        .SelectMany(r => r.RegistroPruebas)
                        .ToListAsync();

                    return View(pruebas);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.RegistroPruebas)
                        .ToListAsync());
                }
            }
            else
            {
                var registroPruebas = await _context.RegistroPruebas
                    .Where(r => r.IdTrabajador == usuarioTrabajador.TrabajadorId).ToListAsync();

                if (registroPruebas.Count == 0)
                    return NotFound("Sin registros de pruebas");

                return View(registroPruebas);
            }

            return View(await _context.RegistroPruebas.ToListAsync());

        }

        // GET: RegistroPruebas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroPrueba = await _context.RegistroPruebas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registroPrueba == null)
            {
                return NotFound();
            }

            return View(registroPrueba);
        }

        // GET: RegistroPruebas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegistroPruebas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Temperatura,PorcentajeO2,TipoSangre,APOlfativa,APGustativa,Mas15cm,Menos15cm,PIE3,PIE4,PIE5,Discriminacion,Total,Diagnostico,ResultadoIgM,ResultadoIgG,ResultadoPCR,IdTrabajador")] RegistroPrueba registroPrueba)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registroPrueba);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registroPrueba);
        }

        // GET: RegistroPruebas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroPrueba = await _context.RegistroPruebas.FindAsync(id);
            if (registroPrueba == null)
            {
                return NotFound();
            }
            return View(registroPrueba);
        }

        // POST: RegistroPruebas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Temperatura,PorcentajeO2,TipoSangre,APOlfativa,APGustativa,Mas15cm,Menos15cm,PIE3,PIE4,PIE5,Discriminacion,Total,Diagnostico,ResultadoIgM,ResultadoIgG,ResultadoPCR,IdTrabajador")] RegistroPrueba registroPrueba)
        {
            if (id != registroPrueba.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registroPrueba);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroPruebaExists(registroPrueba.Id))
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
            return View(registroPrueba);
        }

        // GET: RegistroPruebas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroPrueba = await _context.RegistroPruebas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registroPrueba == null)
            {
                return NotFound();
            }

            return View(registroPrueba);
        }

        // POST: RegistroPruebas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registroPrueba = await _context.RegistroPruebas.FindAsync(id);
            _context.RegistroPruebas.Remove(registroPrueba);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistroPruebaExists(int id)
        {
            return _context.RegistroPruebas.Any(e => e.Id == id);
        }
    }
}
