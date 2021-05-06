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
    public class UsuariosTrabajadoresController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public UsuariosTrabajadoresController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        // GET: UsuariosTrabajadores
        public async Task<IActionResult> Index()
        {
            return View(await _context.UsuariosTrabajadores.ToListAsync());
        }

        // GET: UsuariosTrabajadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioTrabajador = await _context.UsuariosTrabajadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioTrabajador == null)
            {
                return NotFound();
            }

            return View(usuarioTrabajador);
        }

        // GET: UsuariosTrabajadores/Create
        public IActionResult Create()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            UsuarioEmpresa usuarioEmpresa = _context.UsuariosEmpresas.FirstOrDefault(u => u.IdUsuario == Guid.Parse(currentUserId));

            if (currentUserId != null && currentUser.IsInRole("Aministrador"))
            {
                List<DDLUsuarios> usuarios = _context.Users
                .Select(x => new DDLUsuarios { IdUsuario = Guid.Parse(x.Id), Usuario = x.Email }).ToList();
                if (usuarios.Count > 0)
                    ViewBag.Usuarios = usuarios;

                List<DDLTrabajador> trabajadores = _context.Trabajadores
                    .Select(x => new DDLTrabajador { Id = x.IdTrabajador, Trabajador = x.Nombre }).ToList();
                if (trabajadores.Count > 0)
                    ViewBag.Trabajadores = trabajadores;
            }
            else if (currentUserId != null && currentUser.IsInRole("AdminEmpresa"))
            {
            }

            return View();
        }

        // POST: UsuariosTrabajadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TrabajadorId,CodigoEmpresa")] UsuarioTrabajador usuarioTrabajador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioTrabajador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuarioTrabajador);
        }

        // GET: UsuariosTrabajadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioTrabajador = await _context.UsuariosTrabajadores.FindAsync(id);
            if (usuarioTrabajador == null)
            {
                return NotFound();
            }

            List<DDLUsuarios> usuarios = _context.Users
                .Select(x => new DDLUsuarios { IdUsuario = Guid.Parse(x.Id), Usuario = x.Email }).ToList();
            if (usuarios.Count > 0)
                ViewBag.Usuarios = usuarios;

            List<DDLTrabajador> trabajadores = _context.Trabajadores
                .Select(x => new DDLTrabajador { Id = x.IdTrabajador, Trabajador = x.Nombre }).ToList();
            if (trabajadores.Count > 0)
                ViewBag.Trabajadores = trabajadores;

            return View(usuarioTrabajador);
        }

        // POST: UsuariosTrabajadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TrabajadorId,CodigoEmpresa")] UsuarioTrabajador usuarioTrabajador)
        {
            if (id != usuarioTrabajador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioTrabajador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioTrabajadorExists(usuarioTrabajador.Id))
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
            return View(usuarioTrabajador);
        }

        // GET: UsuariosTrabajadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioTrabajador = await _context.UsuariosTrabajadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioTrabajador == null)
            {
                return NotFound();
            }

            return View(usuarioTrabajador);
        }

        // POST: UsuariosTrabajadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioTrabajador = await _context.UsuariosTrabajadores.FindAsync(id);
            _context.UsuariosTrabajadores.Remove(usuarioTrabajador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioTrabajadorExists(int id)
        {
            return _context.UsuariosTrabajadores.Any(e => e.Id == id);
        }
    }
}
