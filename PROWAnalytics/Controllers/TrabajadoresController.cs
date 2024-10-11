using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROWAnalytics.Data;
using PROWAnalytics.Models;
using PROWAnalytics.Responses;
using System.Security.Claims;
using PROWAnalytics.UnParo;

namespace PROWAnalytics.Controllers
{
    public class TrabajadoresController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;
        private readonly ClaimsPrincipal _currentUser;

        public TrabajadoresController(BiproAnalyticsDBContext context)
        {
            _context = context;
            _currentUser = this.User;
        }

        public async Task<IActionResult> Dashboard()
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
                    return NotFound("Usuario no asociado a ninguna empresa");
            }

            return View();
        }

        // GET: Trabajadores
        public async Task<IActionResult> Index(int? IdEmpresa, int? IdTrabajador)
        {
            ClaimsPrincipal currentUser = this.User;
            UsuarioTrabajador usuarioTrabajador = null;
            UsuarioEmpresa usuarioEmpresa = null;
            Empresa empresa = null;

            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (currentUserId != null)
            {
                usuarioTrabajador = await _context.UsuariosTrabajadores.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));
                usuarioEmpresa = await _context.UsuariosEmpresas.FirstOrDefaultAsync(u => u.IdUsuario == Guid.Parse(currentUserId));
            }
                
            if (usuarioTrabajador != null)
                empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);

            if (currentUser.IsInRole("Admin"))
            {

                if (IdEmpresa != null && IdTrabajador == null)
                    return View(await _context.Trabajadores.Where(x => x.IdEmpresa == IdEmpresa)
                        .Include(x=>x.Empresa).Include(x=>x.Unidad).Include(x => x.Area)
                        .ToListAsync());

                if (IdEmpresa == null && IdTrabajador != null)
                    return View(await _context.Trabajadores.Where(x => x.IdTrabajador == IdTrabajador)
                        .Include(x => x.Empresa).Include(x => x.Unidad).Include(x => x.Area)
                        .ToListAsync());

                if (IdEmpresa != null && IdTrabajador != null)
                    return View(await _context.Trabajadores.Where(x => x.IdEmpresa == IdEmpresa && x.IdTrabajador == IdTrabajador)
                        .Include(x => x.Empresa).Include(x => x.Unidad).Include(x => x.Area)
                        .ToListAsync());
                else
                    return View(await _context.Trabajadores
                        .Include(x => x.Empresa).Include(x => x.Unidad).Include(x => x.Area)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (usuarioEmpresa != null)
                {
                    return View(await _context.Trabajadores
                        .Where(x => x.IdEmpresa == usuarioEmpresa.IdEmpresa)
                        .Include(x => x.Empresa).Include(x => x.Unidad).Include(x => x.Area)
                        .ToListAsync());
                }
                else
                {
                    return NotFound();
                }
            }

            return View(await _context.Trabajadores
                .Where(t=> t.IdTrabajador == usuarioTrabajador.TrabajadorId)
                .Include(x => x.Empresa).Include(x => x.Unidad).Include(x => x.Area)
                .ToListAsync());
        }

        // GET: Trabajadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores
                .Include(x => x.Empresa).Include(x => x.Unidad).Include(x => x.Area)
                .FirstOrDefaultAsync(m => m.IdTrabajador == id);

            if (trabajador == null)
            {
                return NotFound();
            }

            return View(trabajador);
        }

        // GET: Trabajadores/Create
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            ViewBag.Unidades = perfilData.DDLUnidades;
            ViewBag.Areas = perfilData.DDLAreas;

            if (currentUser.IsInRole("Admin"))
                ViewBag.Empresas = perfilData.DDLEmpresas;

            if (currentUser.IsInRole("Trabajador"))
            {
                if(perfilData.IdTrabajador != null)
                {
                    return View();
                }
                else
                {
                    ViewBag.TrabajadorCreado = 1;
                }
            }

            return View();
        }

        // POST: Trabajadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTrabajador,Nombre,Genero,Telefono,Correo,Calle,NumeroExt,NumeroInt,Ciudad,Estado,Municipio,CP,FechaNacimiento,FechaIngreso,NombreUnidad,NombreArea,IdEmpresa,NombreEmpresa,IdUnidad,IdArea")] Trabajador trabajador)
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (ModelState.IsValid)
            {
                if (currentUser.IsInRole("Trabajador"))
                {
                    if (perfilData.IdTrabajador == null)
                    {
                        trabajador.FechaRegistro = DateTime.Now;
                        trabajador.Empresa = await _context.Empresas.FindAsync(perfilData.IdEmpresa);
                        _context.Add(trabajador);
                        await _context.SaveChangesAsync();

                        var user = _context.Users.FirstOrDefault(u => u.Email == currentUser.Identity.Name);
                        UsuarioTrabajador usuarioTrabajador = new UsuarioTrabajador
                        {
                            UserId = Guid.Parse(user.Id),
                            TrabajadorId = trabajador.IdTrabajador
                        };
                        _context.UsuariosTrabajadores.Add(usuarioTrabajador);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Forbid("Ya se registró datos de Trabajador");
                    }
                }
                else
                {
                    if (currentUser.IsInRole("AdminEmpresa"))
                        trabajador.Empresa = await _context.Empresas.FindAsync(perfilData.IdEmpresa);

                    trabajador.FechaRegistro = DateTime.Now;
                    _context.Add(trabajador);
                    await _context.SaveChangesAsync();
                }

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

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            ViewBag.Unidades = perfilData.DDLUnidades;
            ViewBag.Areas = perfilData.DDLAreas;
            ViewBag.Empresas = perfilData.DDLEmpresas;

            return View(trabajador);
        }

        // POST: Trabajadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTrabajador,Nombre,Genero,Telefono,Correo,Calle,NumeroExt,NumeroInt,Ciudad,Estado,Municipio,CP,FechaNacimiento,FechaIngreso,NombreUnidad,NombreArea,IdEmpresa,NombreEmpresa,IdUnidad,IdArea")] Trabajador trabajador)
        {
            if (id != trabajador.IdTrabajador)
            {
                return NotFound();
            }

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

        public async Task<IActionResult> VerificaTrabajador()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (perfilData.IdTrabajador != null)
                return RedirectToAction("Details", new { id = perfilData.IdTrabajador });
            else
                return RedirectToAction("Create");

        }

        private bool TrabajadorExists(int id)
        {
            return _context.Trabajadores.Any(e => e.IdTrabajador == id);
        }
    }
}
