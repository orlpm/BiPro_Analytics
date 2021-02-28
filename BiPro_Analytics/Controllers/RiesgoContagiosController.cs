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
    public class RiesgoContagiosController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public RiesgoContagiosController(BiproAnalyticsDBContext context)
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
        // GET: RiesgoContagios
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
                    return View(await _context.RiesgoContagios
                        .Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdUnidad != null && IdArea == null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad)
                        .SelectMany(r => r.RiesgosContagios)
                        .ToListAsync());

                if (IdUnidad == null && IdArea != null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdArea == IdArea)
                        .SelectMany(r => r.RiesgosContagios)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea)
                        .SelectMany(r => r.RiesgosContagios)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                
                if (IdTrabajador != null)
                {
                    return View(await _context.RiesgoContagios
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {

                    List<RiesgoContagio> riesgoContagios = new List<RiesgoContagio>();

                    riesgoContagios = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(t => t.RiesgosContagios).ToListAsync();

                    return View(riesgoContagios);

                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.RiesgosContagios)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var riesgosTrabajadores = await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.RiesgosContagios)
                        .ToListAsync();

                    return View(riesgosTrabajadores);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(a => a.IdArea == IdArea)
                        .SelectMany(r => r.RiesgosContagios)
                        .ToListAsync());
                }
            }
            else
            {
                var riesgoContagios = await _context.RiesgoContagios
                    .Where(r => r.IdTrabajador == perfilData.IdTrabajador).ToListAsync();

                if (riesgoContagios.Count == 0)
                    return NotFound();

                return View(riesgoContagios);
            }

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
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

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
                riesgoContagio.FechaHoraRegistro = DateTime.Now;

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

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

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
                    riesgoContagio.FechaHoraRegistro = DateTime.Now;

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
