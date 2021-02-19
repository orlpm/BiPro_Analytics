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
    public class RiesgoContagiosController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public RiesgoContagiosController(BiproAnalyticsDBContext context)
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
        // GET: RiesgoContagios
        public async Task<IActionResult> Index()
        {
            return View(await _context.RiesgoContagios.ToListAsync());
        }

        // GET: RiesgoContagios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riesgoContagio = await _context.RiesgoContagios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (riesgoContagio == null)
            {
                return NotFound();
            }

            return View(riesgoContagio);
        }

        // GET: RiesgoContagios/Create
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

            if (trabajadores.Count > 0)
                ViewBag.Trabajadores = trabajadores;

            return View();
        }

        // POST: RiesgoContagios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContactoCovidCasa,ContactoCovidTrabajo,ContactoCovidFuera,ViajesMultitudes,TosRecurrente,Tos,DificultadRespirar,TempMayor38,Resfriado,Escalofrios,DolorMuscular,NauseaVomito,Diarrea,Olfatometria,IdTrabajador")] RiesgoContagio riesgoContagio)
        {
            if (ModelState.IsValid)
            {
                riesgoContagio.Trabajador = _context.Trabajadores.Find(riesgoContagio.IdTrabajador);

                _context.Add(riesgoContagio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(riesgoContagio);
        }

        // GET: RiesgoContagios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riesgoContagio = await _context.RiesgoContagios.FindAsync(id);
            if (riesgoContagio == null)
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

            if (trabajadores.Count > 0)
                ViewBag.Trabajadores = trabajadores;

            return View(riesgoContagio);
        }

        // POST: RiesgoContagios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContactoCovidCasa,ContactoCovidTrabajo,ContactoCovidFuera,ViajesMultitudes,TosRecurrente,Tos,DificultadRespirar,TempMayor38,Resfriado,Escalofrios,DolorMuscular,NauseaVomito,Diarrea,Olfatometria,IdTrabajador")] RiesgoContagio riesgoContagio)
        {
            if (id != riesgoContagio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    riesgoContagio.Trabajador = _context.Trabajadores.Find(riesgoContagio.IdTrabajador);
                    _context.Update(riesgoContagio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RiesgoContagioExists(riesgoContagio.Id))
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
            return View(riesgoContagio);
        }

        // GET: RiesgoContagios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riesgoContagio = await _context.RiesgoContagios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (riesgoContagio == null)
            {
                return NotFound();
            }

            return View(riesgoContagio);
        }

        // POST: RiesgoContagios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var riesgoContagio = await _context.RiesgoContagios.FindAsync(id);
            _context.RiesgoContagios.Remove(riesgoContagio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RiesgoContagioExists(int id)
        {
            return _context.RiesgoContagios.Any(e => e.Id == id);
        }
    }
}
