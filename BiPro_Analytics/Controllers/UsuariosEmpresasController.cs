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

namespace BiPro_Analytics.Controllers
{
    public class UsuariosEmpresasController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public UsuariosEmpresasController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        // GET: UsuariosEmpresas
        public async Task<IActionResult> Index()
        {
            return View(await _context.UsuariosEmpresas.ToListAsync());
        }

        // GET: UsuariosEmpresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioEmpresa = await _context.UsuariosEmpresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioEmpresa == null)
            {
                return NotFound();
            }

            return View(usuarioEmpresa);
        }

        // GET: UsuariosEmpresas/Create
        public IActionResult Create()
        {


            List<DDLUsuarios> usuarios = _context.Users
                .Select(x => new DDLUsuarios { IdUsuario = Guid.Parse(x.Id), Usuario = x.Email }).ToList();
            if (usuarios.Count > 0)
                ViewBag.Usuarios = usuarios;

            List<DDLEmpresa> empresas = _context.Empresas
                .Select(x => new DDLEmpresa { Id = x.IdEmpresa, Empresa = x.Nombre }).ToList();
            if (empresas.Count > 0)
                ViewBag.Empresas = empresas;


            return View();
        }

        // POST: UsuariosEmpresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUsuario,IdEmpresa,Rol")] UsuarioEmpresa usuarioEmpresa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioEmpresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuarioEmpresa);
        }

        // GET: UsuariosEmpresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioEmpresa = await _context.UsuariosEmpresas.FindAsync(id);
            if (usuarioEmpresa == null)
            {
                return NotFound();
            }

            List<DDLEmpresa> empresas = await _context.Empresas
                .Select(x => new DDLEmpresa { Id = x.IdEmpresa, Empresa = x.Nombre }).ToListAsync();
            if (empresas.Count > 0)
                ViewBag.Empresas = empresas;

            List<DDLUsuarios> usuarios = _context.Users
                .Select(x => new DDLUsuarios { IdUsuario = Guid.Parse(x.Id), Usuario = x.Email }).ToList();
            if (usuarios.Count > 0)
                ViewBag.Usuarios = usuarios;

            return View(usuarioEmpresa);
        }

        // POST: UsuariosEmpresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUsuario,IdEmpresa,Rol")] UsuarioEmpresa usuarioEmpresa)
        {
            if (id != usuarioEmpresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioEmpresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioEmpresaExists(usuarioEmpresa.Id))
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
            return View(usuarioEmpresa);
        }

        // GET: UsuariosEmpresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioEmpresa = await _context.UsuariosEmpresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioEmpresa == null)
            {
                return NotFound();
            }

            return View(usuarioEmpresa);
        }

        // POST: UsuariosEmpresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioEmpresa = await _context.UsuariosEmpresas.FindAsync(id);
            _context.UsuariosEmpresas.Remove(usuarioEmpresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioEmpresaExists(int id)
        {
            return _context.UsuariosEmpresas.Any(e => e.Id == id);
        }
    }
}
