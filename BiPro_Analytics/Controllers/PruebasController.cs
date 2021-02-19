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

namespace BiPro_Analytics.Controllers
{
    public class PruebasController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public PruebasController(BiproAnalyticsDBContext context)
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
        // GET: Pruebas
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

            if (usuarioTrabajador != null)
            {
                empresa = await _context.Empresas
                    .FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);
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
                List<Trabajador> trabajadores;

                if (IdTrabajador != null)
                {
                    return View(await _context.Pruebas
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == empresa.IdEmpresa).ToListAsync();

                    if (trabajadores.Count > 0)
                    {
                        List<Prueba> pruebas= new List<Prueba>();

                        foreach (var trabajador in trabajadores)
                        {
                            var prueba = await _context.Pruebas
                                .FirstOrDefaultAsync(x => x.IdTrabajador == trabajador.IdTrabajador);

                            if (prueba != null)
                                pruebas.Add(prueba);
                        }

                        if (pruebas.Count > 0)
                            return View(pruebas);
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
                        .SelectMany(r => r.Pruebas)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    //var riesgos = await _context.Unidades
                    //    .Where(u => u.Id == IdUnidad)
                    //    .SelectMany(t => t.Trabajadores)
                    //    .ToListAsync();

                    var riesgos = await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.Pruebas)
                        .ToListAsync();

                    return View(riesgos);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.Pruebas)
                        .ToListAsync());
                }
            }
            else
            {
                var riesgosTrabajadores = await _context.RiesgosTrabajadores
                    .Where(r => r.IdTrabajador == usuarioTrabajador.TrabajadorId).ToListAsync();

                if (riesgosTrabajadores.Count == 0)
                    return NotFound("Sin encuesta de riesgos");

                return View(riesgosTrabajadores);
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
        public IActionResult Create()
        {
            //Para combo Trabajadores
            List<DDLTrabajador> trabajadores = null;
            trabajadores = _context.Trabajadores
                    .Select(x => new DDLTrabajador
                    {
                        Id = x.IdTrabajador,
                        Trabajador = x.Nombre
                    }).ToList();

            return View();
        }

        // POST: Pruebas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaDiagnostico,Lugar,TipoPrueba,DiagnosticoCovid,PruebaConfirmatoria,SintomasCovid,RadiografiaTorax,Tomografía,IdTrabajador")] Prueba prueba)
        {
            if (ModelState.IsValid)
            {
                prueba.Trabajador = _context.Trabajadores.Find(prueba.IdTrabajador);
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

            //Para combo Trabajadores
            List<DDLTrabajador> trabajadores = null;
            trabajadores = _context.Trabajadores
                    .Select(x => new DDLTrabajador
                    {
                        Id = x.IdTrabajador,
                        Trabajador = x.Nombre
                    }).ToList();

            return View(prueba);
        }

        // POST: Pruebas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaDiagnostico,Lugar,TipoPrueba,DiagnosticoCovid,PruebaConfirmatoria,SintomasCovid,RadiografiaTorax,Tomografía,IdTrabajador")] Prueba prueba)
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
