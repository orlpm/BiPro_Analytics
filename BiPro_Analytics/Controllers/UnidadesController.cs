using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiPro_Analytics.Data;
using BiPro_Analytics.Models;
using BiPro_Analytics.Responses;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BiPro_Analytics.UnParo;

namespace BiPro_Analytics.Controllers
{
    [Authorize(Roles = "Admin,AdminEmpresa")]
    public class UnidadesController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public UnidadesController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        // GET: Unidades
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            ViewBag.Unidades = perfilData.DDLUnidades;
            ViewBag.Areas = perfilData.DDLAreas;
            ViewBag.Empresas = perfilData.DDLEmpresas;

            var empresa = await _context.Empresas.FindAsync(perfilData.IdEmpresa);

            if (empresa == null)
                return NotFound("Empresa no encotrada");

            ViewBag.SinUnidades = empresa.DescartarUnidades;

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                    return NotFound("Usuario no asociado a ninguna empresa");

                return View(await _context.Unidades.Where(u => u.IdEmpresa == perfilData.IdEmpresa).ToListAsync());
            }

            return View(await _context.Unidades.ToListAsync());
        }

        // GET: Unidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidad = await _context.Unidades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unidad == null)
            {
                return NotFound();
            }

            return View(unidad);
        }

        // GET: Unidades/Create
        public async Task<IActionResult> Create()
        {
            List<DDLEmpresa> empresas = await _context.Empresas
                    .Select(x => new DDLEmpresa { Id = x.IdEmpresa, Empresa = x.Nombre }).ToListAsync();
            ViewBag.Empresas = empresas;

            return View();
        }

        // POST: Unidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,IdEmpresa")] Unidad unidad)
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            Empresa empresa;

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                empresa = await _context.Empresas.FindAsync(perfilData.IdEmpresa);
                unidad.IdEmpresa = (int)perfilData.IdEmpresa;
            }
            else
            {
                empresa = await _context.Empresas.FindAsync(unidad.IdEmpresa);
            }

            if (empresa == null)
                return NotFound("Empresa no encontrada");

            unidad.Empresa = empresa;

            if (ModelState.IsValid)
            {
                _context.Add(unidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(unidad);
        }

        // GET: Unidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidad = await _context.Unidades.FindAsync(id);
            if (unidad == null)
            {
                return NotFound();
            }
            return View(unidad);
        }

        // POST: Unidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,IdEmpresa")] Unidad unidad)
        {
            if (id != unidad.Id)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            Empresa empresa;

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                empresa = await _context.Empresas.FindAsync(perfilData.IdEmpresa);
                unidad.IdEmpresa = (int)perfilData.IdEmpresa;
            }
            else
            {
                empresa = await _context.Empresas.FindAsync(unidad.IdEmpresa);
            }

            if (empresa == null)
                return NotFound("Empresa no encontrada");

            unidad.Empresa = empresa;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnidadExists(unidad.Id))
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
            return View(unidad);
        }

        // GET: Unidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidad = await _context.Unidades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unidad == null)
            {
                return NotFound();
            }

            return View(unidad);
        }

        // POST: Unidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unidad = await _context.Unidades.FindAsync(id);
            _context.Unidades.Remove(unidad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnidadExists(int id)
        {
            return _context.Unidades.Any(e => e.Id == id);
        }

        public async Task<IActionResult> IndicarSinUnidades()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            var empresa = await _context.Empresas.FindAsync(perfilData.IdEmpresa);

            if (empresa.DescartarUnidades)
                empresa.DescartarUnidades = false;
            else
                empresa.DescartarUnidades = true;

            _context.Empresas.Update(empresa);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
