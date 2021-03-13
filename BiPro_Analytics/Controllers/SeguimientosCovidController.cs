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
    public class SeguimientosCovidController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public SeguimientosCovidController(BiproAnalyticsDBContext context)
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
                {
                    return NotFound("Datos de empresa no encontrados");
                }
            }

            return View();
        }
        // GET: SeguimientosCovid

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
                    return View(await _context.SeguimientosCovid
                        .Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdUnidad != null)
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {

                if (IdTrabajador != null)
                {
                    return View(await _context.SeguimientosCovid
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {

                    List<SeguimientoCovid> seguimientosCovid = new List<SeguimientoCovid>();

                    seguimientosCovid = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(t => t.SeguimientosCovid).ToListAsync();

                    return View(seguimientosCovid);

                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var seguimientosCovid = await _context.Trabajadores
                        .Where(u => u.IdUnidad == IdUnidad)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync();

                    return View(seguimientosCovid);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(a => a.IdArea == IdArea)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync());
                }
            }
            else
            {
                var seguimientosCovid = await _context.SeguimientosCovid
                    .Where(r => r.IdTrabajador == perfilData.IdTrabajador).ToListAsync();

                if (seguimientosCovid.Count == 0)
                    return NotFound("Trabajador sin seguimiento covid");

                return View(seguimientosCovid);
            }

            return View(await _context.SeguimientosCovid.ToListAsync());

        }

        // GET: SeguimientosCovid/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Create
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            return View();
        }

        // POST: SeguimientosCovid/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EstatusPaciente,SintomasMayores,SintomasMenores,EstatusEnCasa,EstatusEnHospital,FechaSeguimiento,IdTrabajador")] SeguimientoCovid seguimientoCovid)
        {
            if (ModelState.IsValid)
            {
                seguimientoCovid.Trabajador = _context.Trabajadores.Find(seguimientoCovid.IdTrabajador);
                _context.Add(seguimientoCovid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid.FindAsync(id);
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            return View(seguimientoCovid);
        }

        // POST: SeguimientosCovid/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EstatusPaciente,SintomasMayores,SintomasMenores,EstatusEnCasa,EstatusEnHospital,FechaSeguimiento,IdTrabajador")] SeguimientoCovid seguimientoCovid)
        {
            if (id != seguimientoCovid.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    seguimientoCovid.Trabajador = _context.Trabajadores.Find(seguimientoCovid.IdTrabajador);
                    _context.Update(seguimientoCovid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeguimientoCovidExists(seguimientoCovid.Id))
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
            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            return View(seguimientoCovid);
        }

        // POST: SeguimientosCovid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seguimientoCovid = await _context.SeguimientosCovid.FindAsync(id);
            _context.SeguimientosCovid.Remove(seguimientoCovid);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeguimientoCovidExists(int id)
        {
            return _context.SeguimientosCovid.Any(e => e.Id == id);
        }
    }
}
