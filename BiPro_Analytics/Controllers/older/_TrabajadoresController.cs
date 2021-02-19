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
    public class _TrabajadoresController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;
        private readonly ClaimsPrincipal _currentUser;

        public _TrabajadoresController(BiproAnalyticsDBContext context)
        {
            _context = context;
            _currentUser = this.User;
        }

        public async Task<IActionResult> PreIndex()
        {
            ClaimsPrincipal currentUser = this.User;
            Empresa empresa = null;
            UsuarioTrabajador usuarioTrabajador = null;

            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserId != null)
                usuarioTrabajador = await _context.UsuariosTrabajadores.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));

            if (usuarioTrabajador != null)
                empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);

            if (currentUser.IsInRole("Admin"))
            {
                List<DDLEmpresa> empresas = await _context.Empresas
                    .Select(x => new DDLEmpresa { Id = x.IdEmpresa, Empresa = x.Nombre}).ToListAsync();
                ViewBag.Empresas = empresas;
                
                List<DDLTrabajador> trabajadores = await _context.Trabajadores
                    .Select(x => new DDLTrabajador { Id = x.IdTrabajador, Trabajador = x.Nombre }).ToListAsync();
                ViewBag.Trabajadores = trabajadores;
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                if(empresa!= null)
                {
                    List<DDLEmpresa> empresas = await _context.Empresas
                    .Where(e => e.IdEmpresa == empresa.IdEmpresa)
                    .Select(x => new DDLEmpresa { Id = x.IdEmpresa, Empresa = x.Nombre}).ToListAsync();
                    ViewBag.Empresas = empresas;

                    List<DDLTrabajador> trabajadores = await _context.Trabajadores
                        .Where(e => e.IdEmpresa == empresa.IdEmpresa)
                        .Select(x => new DDLTrabajador { Id = x.IdTrabajador, Trabajador = x.Nombre })
                        .ToListAsync();
                    ViewBag.Trabajadores = trabajadores;
                }
                else
                {
                    return NotFound();
                }
            }

            return View();
        }
        // GET: Trabajadores
        public async Task<IActionResult> Index(int? IdEmpresa, int? IdTrabajador)
        {
            ClaimsPrincipal currentUser = this.User;
            UsuarioTrabajador usuarioTrabajador = null;
            Empresa empresa = null;

            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(currentUserId != null)
                usuarioTrabajador = await _context.UsuariosTrabajadores.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));
            if(usuarioTrabajador!=null)
                empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);

            if (currentUser.IsInRole("Admin"))
            {
                if (IdEmpresa != null && IdTrabajador == null)
                    return View(await _context.Trabajadores.Where(x => x.IdEmpresa == IdEmpresa).ToListAsync());

                if (IdEmpresa == null && IdTrabajador != null)
                    return View(await _context.Trabajadores.Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdEmpresa != null && IdTrabajador != null)
                    return View(await _context.Trabajadores.Where(x => x.IdEmpresa == IdEmpresa && x.IdTrabajador == IdTrabajador).ToListAsync());
                else
                    return View(await _context.Trabajadores.ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (empresa != null)
                {
                    return View(await _context.Trabajadores
                        .Where(x => x.IdEmpresa == empresa.IdEmpresa ).ToListAsync());
                }
                else
                {
                    return NotFound();
                }
            }

            return View(await _context.Trabajadores.ToListAsync());
        }

        // GET: Trabajadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores
                .FirstOrDefaultAsync(m => m.IdTrabajador == id);
            if (trabajador == null)
            {
                return NotFound();
            }

            return View(trabajador);
        }

        // GET: Trabajadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trabajadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTrabajador,Nombre,Telefono,Correo,Ciudad,CP,FechaNacimiento,Genero,Area,Unidad,IdEmpresa")] Trabajador trabajador)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var usuarioTrabajador = await _context.UsuariosTrabajadores.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));
            usuarioTrabajador.TrabajadorId = trabajador.IdTrabajador;

            var empresa = await _context.Empresas.FirstOrDefaultAsync(i => i.IdEmpresa == trabajador.IdEmpresa);

            if (empresa != null)
            {
                trabajador.Empresa = empresa;
                trabajador.NombreEmpresa = empresa.Nombre;
            }
            else
                return NotFound("Empresa no vinculada");

            if (ModelState.IsValid)
            {
                _context.Add(trabajador);
                _context.Update(usuarioTrabajador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trabajador);
        }

        // GET: Trabajadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores.FindAsync(id);
            if (trabajador == null)
            {
                return NotFound();
            }
            return View(trabajador);
        }

        // POST: Trabajadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTrabajador,Nombre,Telefono,Correo,Ciudad,CP,FechaNacimiento,Genero,Unidad,IdEmpresa,IdEmpresa")] Trabajador trabajador)
        {
            if (id != trabajador.IdTrabajador)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FirstOrDefaultAsync(i => i.IdEmpresa == trabajador.IdEmpresa);

            if (empresa != null)
                trabajador.Empresa = empresa;
            else
                return NotFound();


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trabajador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrabajadorExists(trabajador.IdTrabajador))
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
            return View(trabajador);
        }

        // GET: Trabajadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores
                .FirstOrDefaultAsync(m => m.IdTrabajador == id);
            if (trabajador == null)
            {
                return NotFound();
            }

            return View(trabajador);
        }

        // POST: Trabajadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trabajador = await _context.Trabajadores.FindAsync(id);
            _context.Trabajadores.Remove(trabajador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrabajadorExists(int id)
        {
            return _context.Trabajadores.Any(e => e.IdTrabajador == id);
        }
    }
}
