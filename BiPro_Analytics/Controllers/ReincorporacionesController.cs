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
    public class ReincorporacionesController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public ReincorporacionesController(BiproAnalyticsDBContext context)
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
        // GET: Reincorporaciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reincorporaciones.ToListAsync());
        }

        // GET: Reincorporaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reincorporaciones = await _context.Reincorporaciones
                .FirstOrDefaultAsync(m => m.id == id);
            if (reincorporaciones == null)
            {
                return NotFound();
            }

            return View(reincorporaciones);
        }

        // GET: Reincorporaciones/Create
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

        // POST: Reincorporaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Numero,Aislados15Dias,Aislados30Dias,ReincorporanSemAnt,EmpleadosIncapacidad,DiasPerdidosIncapacidadSemAnt,DiasAcumuladosMenIncapacidad,DiasAcumuladosTot,RelTotalTrabajadoresTrabajadoresIncapacidad,RelDiasTrabajoDiasIncapacidad,IdEmpresa")] Reincorporaciones reincorporaciones)
        {
            if (ModelState.IsValid)
            {
                reincorporaciones.Trabajador = _context.Trabajadores.Find(reincorporaciones.IdTrabajador);
                _context.Add(reincorporaciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reincorporaciones);
        }

        // GET: Reincorporaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reincorporaciones = await _context.Reincorporaciones.FindAsync(id);
            if (reincorporaciones == null)
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

            return View(reincorporaciones);
        }

        // POST: Reincorporaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Numero,Aislados15Dias,Aislados30Dias,ReincorporanSemAnt,EmpleadosIncapacidad,DiasPerdidosIncapacidadSemAnt,DiasAcumuladosMenIncapacidad,DiasAcumuladosTot,RelTotalTrabajadoresTrabajadoresIncapacidad,RelDiasTrabajoDiasIncapacidad,IdEmpresa")] Reincorporaciones reincorporaciones)
        {
            if (id != reincorporaciones.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    reincorporaciones.Trabajador = _context.Trabajadores.Find(reincorporaciones.IdTrabajador);
                    _context.Update(reincorporaciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReincorporacionesExists(reincorporaciones.id))
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
            return View(reincorporaciones);
        }

        // GET: Reincorporaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reincorporaciones = await _context.Reincorporaciones
                .FirstOrDefaultAsync(m => m.id == id);
            if (reincorporaciones == null)
            {
                return NotFound();
            }

            return View(reincorporaciones);
        }

        // POST: Reincorporaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reincorporaciones = await _context.Reincorporaciones.FindAsync(id);
            _context.Reincorporaciones.Remove(reincorporaciones);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReincorporacionesExists(int id)
        {
            return _context.Reincorporaciones.Any(e => e.id == id);
        }
    }
}
