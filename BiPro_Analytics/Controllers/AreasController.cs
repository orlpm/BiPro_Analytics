using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiPro_Analytics.Data;
using BiPro_Analytics.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BiPro_Analytics.UnParo;

namespace BiPro_Analytics.Controllers
{
    [Authorize(Roles = "Admin,AdminEmpresa")]
    public class AreasController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public AreasController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        // GET: Areas
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

            ViewBag.SinAreas = empresa.DescartarAreas;

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                    return NotFound("Usuario no asociado a ninguna empresa");

                return View(await _context.Areas.Where(u => u.IdEmpresa == perfilData.IdEmpresa).ToListAsync());
            }

            return View(await _context.Areas.ToListAsync());
        }

        // GET: Areas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // GET: Areas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Areas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,IdEmpresa")] Area area)
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            Empresa empresa;

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                empresa = await _context.Empresas.FindAsync(perfilData.IdEmpresa);
                area.IdEmpresa = (int)perfilData.IdEmpresa;
            }
            else
            {
                empresa = await _context.Empresas.FindAsync(area.IdEmpresa);
            }

            if (empresa == null)
                return NotFound("Empresa no encontrada");

            area.Empresa = empresa;

            if (ModelState.IsValid)
            {
                _context.Add(area);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(area);
        }

        // GET: Areas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,IdEmpresa")] Area area)
        {
            if (id != area.Id)
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
                area.IdEmpresa = (int)perfilData.IdEmpresa;
            }
            else
            {
                empresa = await _context.Empresas.FindAsync(area.IdEmpresa);
            }

            if (empresa == null)
                return NotFound("Empresa no encontrada");

            area.Empresa = empresa;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(area);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(area.Id))
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
            return View(area);
        }

        // GET: Areas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AreaExists(int id)
        {
            return _context.Areas.Any(e => e.Id == id);
        }


        public async Task<IActionResult> IndicarSinAreas()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            var empresa = await _context.Empresas.FindAsync(perfilData.IdEmpresa);

            if(empresa.DescartarAreas)
                empresa.DescartarAreas = false;
            else
                empresa.DescartarAreas = true;

            _context.Empresas.Update(empresa);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
