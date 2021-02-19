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
    public class EmpresasController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public EmpresasController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PreIndex(int? idUnidad)
        {
            List<DDLEmpresa> empresas = await _context.Empresas
                .Select(x => new DDLEmpresa
                {
                    Id = x.IdEmpresa,
                    Empresa = x.Nombre
                }).ToListAsync();

            ViewBag.Empresas = empresas;

            List<Area> areas = await _context.Areas
                .ToListAsync();

            //if (idUnidad != null)
            //{
            //    List<Unidad> unidades = await _context.Unidades
            //    .Where(u => u == idArea).ToListAsync();
            //}

            return View();
        }

        // GET: Empresas
        public async Task<IActionResult> Index(int? IdEmpresa)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            ViewBag.IdEmpresa = IdEmpresa;

            if (User.IsInRole("Admin"))
            {
                if (IdEmpresa != null)
                    return View(await _context.Empresas.Where(x => x.IdEmpresa == IdEmpresa).ToListAsync());
                else
                    return View(await _context.Empresas.ToListAsync());
            }
            else
            {
                var usuarioTrabajador = await _context.UsuariosTrabajadores.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));
                var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);

                return View(await _context.Empresas.Where(x => x.IdEmpresa == empresa.IdEmpresa).ToListAsync());
            }
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.IdEmpresa == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa,Nombre,RazonSocial,RFC,Aministrador,Puesto,Giro,SubGiro,Seccion,Telefono,Correo,Calle,NumeroExt,NumeroInt,Ciudad,Estado,CP,CantEmpleados,MinSueldo,MaxSueldo,FechaRegistro,HorasLaborales,DiasLaborales,CodigoEmpresa")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                Random rdn = new Random();
                string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                int longitud = caracteres.Length;
                char letra;
                int longitudContrasenia = 10;
                string codigoAleatorio = string.Empty;

                for (int i = 0; i < longitudContrasenia; i++)
                {
                    letra = caracteres[rdn.Next(longitud)];
                    codigoAleatorio += letra.ToString();
                }

                empresa.CodigoEmpresa = codigoAleatorio;


                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpresa,Nombre,RazonSocial,RFC,Aministrador,Puesto,Giro,SubGiro,Seccion,Telefono,Correo,Calle,NumeroExt,NumeroInt,Ciudad,Estado,CP,CantEmpleados,MinSueldo,MaxSueldo,FechaRegistro,HorasLaborales,DiasLaborales,CodigoEmpresa")] Empresa empresa)
        {
            if (id != empresa.IdEmpresa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.IdEmpresa))
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
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.IdEmpresa == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresas.Any(e => e.IdEmpresa == id);
        }
    }
}
