using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROWAnalytics.Data;
using PROWAnalytics.Models;
using System.Security.Claims;
using PROWAnalytics.Responses;
using PROWAnalytics.UnParo;
using Microsoft.AspNetCore.Authorization;

namespace PROWAnalytics.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public EmpresasController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,AdminEmpresa")]
        public async Task<IActionResult> Dashboard(int? idUnidad)
        {
            ClaimsPrincipal currentUser = this.User;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (perfilData.IdEmpresa != null)
                return RedirectToAction("Index", new { IdEmpresa = perfilData.IdEmpresa });

            List<DDLEmpresa> empresas = await _context.Empresas
                .Select(x => new DDLEmpresa
                {
                    Empresa = x.Nombre
                }).ToListAsync();

            ViewBag.Empresas = empresas;
            ViewBag.IdEmpresa = perfilData.IdEmpresa;

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
        [Authorize(Roles = "Admin,AdminEmpresa")]
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
                {
                    var empresasList = await _context.Empresas.ToListAsync();//Regresa los registros de empresa dentro de una lista de empresas
                    
                    return View(empresasList);
                }
            }
            else
            {
                var usuarioEmpresa = await _context.UsuariosEmpresas.FirstOrDefaultAsync(u => u.IdUsuario == Guid.Parse(currentUserId));
                //var usuarioTrabajador = await _context.UsuariosTrabajadores.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));

                //if(usuarioEmpresa != null)
                //    var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);


                if (usuarioEmpresa != null)
                    return View(await _context.Empresas.Where(x => x.IdEmpresa == usuarioEmpresa.IdEmpresa).ToListAsync());
                else
                    return View(NotFound("Usuario sin relación a empresa"));
            }
        }

        // GET: Empresas/Details/5
        [Authorize(Roles = "Admin,AdminEmpresa")]
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
        [Authorize(Roles = "Admin,AdminEmpresa")]
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                {
                    return View();
                }
                else
                {
                    ViewBag.EmpresaCreada = 1;
                }
            }

            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,AdminEmpresa")]
        public async Task<IActionResult> Create([Bind("IdEmpresa,Nombre,RazonSocial,RFC,ActividadPrincipal,Sector,Calle,NumeroExt,NumeroInt,Colonia,CiudadMunicipio,Estado,CP,Aministrador,Puesto,Telefono,Correo,CantidadEmpleados,SueldoMinimo,SueldoMaximo,SueldoPromedio,NumeroSucursales,Comedor,TransporteTrabajadores,ServicioMedico,SGMM,TrabajadoresConSGMM,NombreAseguradora,NombreAgenteSeguros,HorasLaborales,DiasLaborales")] Empresa empresa)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

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
                empresa.FechaRegistro = DateTime.Now;


                if (currentUser.IsInRole("AdminEmpresa"))
                {
                    if (perfilData.IdEmpresa == null)
                    {
                        _context.Add(empresa);
                        await _context.SaveChangesAsync();
                        UsuarioEmpresa usuarioEmpresa = new UsuarioEmpresa();
                        usuarioEmpresa.IdEmpresa = empresa.IdEmpresa;
                        usuarioEmpresa.IdUsuario = Guid.Parse(currentUserId);

                        _context.Add(usuarioEmpresa);
                        await _context.SaveChangesAsync();
                        ViewBag.EmpresaCreada = 2;
                    }
                    else
                    {
                        return Forbid("Ya se registró datos de empresa");
                    }
                }
                else
                {
                    _context.Add(empresa);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Home");
                //return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpresa,Nombre,RazonSocial,RFC,ActividadPrincipal,Sector,Calle,NumeroExt,NumeroInt,Colonia,CiudadMunicipio,Estado,CP,Aministrador,Puesto,Telefono,Correo,CantidadEmpleados,SueldoMinimo,SueldoMaximo,SueldoPromedio,NumeroSucursales,Comedor,TransporteTrabajadores,ServicioMedico,SGMM,TrabajadoresConSGMM,NombreAseguradora,NombreAgenteSeguros,HorasLaborales,DiasLaborales")] Empresa empresa)
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
