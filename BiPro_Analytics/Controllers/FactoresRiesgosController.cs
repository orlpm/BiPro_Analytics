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
using Microsoft.AspNetCore.Http;
using BiPro_Analytics.Responses;
using BiPro_Analytics.UnParo;

namespace BiPro_Analytics.Controllers
{
    public class FactoresRiesgosController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public FactoresRiesgosController(BiproAnalyticsDBContext context)
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

        // GET: FactoresRiesgos
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
                    return View(await _context.FactoresRiesgos
                        .Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdUnidad != null && IdArea == null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync());

                if (IdUnidad == null && IdArea != null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdArea == IdArea)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (IdTrabajador != null)
                {
                    return View(await _context.FactoresRiesgos
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {

                    List<FactorRiesgo> factoresRiesgos = new List<FactorRiesgo>();

                    factoresRiesgos = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(t => t.FactoresRiesgos).ToListAsync();

                    return View(factoresRiesgos);

                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var riesgos = await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync();

                    return View(riesgos);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdArea == IdArea && t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync());
                }
            }
            else
            {
                var factoresRiesgos = await _context.FactoresRiesgos
                    .Where(r => r.IdTrabajador == perfilData.IdTrabajador).ToListAsync();

                if (factoresRiesgos.Count == 0)
                    return NotFound();

                return View(factoresRiesgos);
            }

            return View(await _context.FactoresRiesgos.ToListAsync());

        }

        // GET: FactoresRiesgos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factorRiesgo = await _context.FactoresRiesgos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factorRiesgo == null)
            {
                return NotFound();
            }

            return View(factorRiesgo);
        }

        // GET: FactoresRiesgos/Create
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //ViewBag.Unidades = perfilData.DDLUnidades;
            //ViewBag.Areas = perfilData.DDLAreas;
            //ViewBag.Empresas = perfilData.DDLEmpresas;

            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            return View();
        }

        // POST: FactoresRiesgos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Diabetes,Hipertension,Asma,SobrePeso,Obesidad,Embarazo,Cancer,Tabaquismo,Alcoholismo,Drogas,NoPersonasCasa,TipoCasa,TipoTransporte,EspacioTrabajo,TipoVentilacion,ContactoLaboral,TiempoContacto,IdTrabajador")] FactorRiesgo factorRiesgo)
        {
            if (ModelState.IsValid)
            {
                factorRiesgo.Trabajador = _context.Trabajadores.Find(factorRiesgo.IdTrabajador);
                factorRiesgo.FechaHoraRegistro = DateTime.Now;

                _context.Add(factorRiesgo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(factorRiesgo);
        }

        // GET: FactoresRiesgos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factorRiesgo = await _context.FactoresRiesgos.FindAsync(id);
            if (factorRiesgo == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            return View(factorRiesgo);
        }

        // POST: FactoresRiesgos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Diabetes,Hipertension,Asma,SobrePeso,Obesidad,Embarazo,Cancer,Tabaquismo,Alcoholismo,Drogas,NoPersonasCasa,TipoCasa,TipoTransporte,EspacioTrabajo,TipoVentilacion,ContactoLaboral,TiempoContacto,IdTrabajador")] FactorRiesgo factorRiesgo)
        {
            if (id != factorRiesgo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    factorRiesgo.Trabajador = _context.Trabajadores.Find(factorRiesgo.IdTrabajador);
                    factorRiesgo.FechaHoraRegistro = DateTime.Now;
                    _context.Update(factorRiesgo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactorRiesgoExists(factorRiesgo.Id))
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
            return View(factorRiesgo);
        }

        // GET: FactoresRiesgos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factorRiesgo = await _context.FactoresRiesgos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factorRiesgo == null)
            {
                return NotFound();
            }

            return View(factorRiesgo);
        }

        // POST: FactoresRiesgos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var factorRiesgo = await _context.FactoresRiesgos.FindAsync(id);
            _context.FactoresRiesgos.Remove(factorRiesgo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactorRiesgoExists(int id)
        {
            return _context.FactoresRiesgos.Any(e => e.Id == id);
        }
    }
}
