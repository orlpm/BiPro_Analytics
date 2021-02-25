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

        public async Task<IActionResult> PreIndex(int? IdUnidad, int? IdArea)
        {
            ClaimsPrincipal currentUser = this.User;
            Empresa empresa = null;
            UsuarioTrabajador usuarioTrabajador = null;
            UsuarioEmpresa usuarioEmpresa = null;
            List<DDLTrabajador> trabajadores = null;
            List<Unidad> unidades = null;
            List<Area> areas = null;

            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUserId != null)
            {
                usuarioTrabajador = await _context.UsuariosTrabajadores.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));
                usuarioEmpresa = await _context.UsuariosEmpresas.FirstOrDefaultAsync(u => u.IdUsuario == Guid.Parse(currentUserId));
            }
                

            if (usuarioTrabajador != null)
                empresa = await _context.Empresas
                    .FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);

            if (currentUser.IsInRole("Admin"))
            {
                trabajadores = _context.Trabajadores
                    .Select(x => new DDLTrabajador
                    {
                        Id = x.IdTrabajador,
                        Trabajador = x.Nombre
                    }).ToList();

                ViewBag.Trabajadores = trabajadores;

                //unidades = await _context.Unidades.Where(u => u.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                unidades = await _context.Unidades.ToListAsync();
                areas = await _context.Areas.ToListAsync();

                ViewBag.Unidades = unidades;
                ViewBag.areas = areas;
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (usuarioEmpresa != null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == usuarioEmpresa.IdEmpresa)
                        .Select(x => new DDLTrabajador
                        {
                            Id = x.IdTrabajador,
                            Trabajador = x.Nombre
                        }).ToListAsync();

                    ViewBag.Trabajadores = trabajadores;

                    unidades = await _context.Unidades.Where(u => u.IdEmpresa == usuarioEmpresa.IdEmpresa).ToListAsync();
                    ViewBag.Unidades = unidades;

                    if (IdUnidad != null)
                    {
                        ViewBag.Unidad = IdUnidad;
                    }

                    areas = await _context.Areas.Where(a => a.IdEmpresa == usuarioEmpresa.IdEmpresa).ToListAsync();
                    ViewBag.Areas = areas;

                }
                else
                {
                    return NotFound("Datos de empresa no encontrados");
                }
            }
            else if (currentUser.IsInRole("Trabajador"))
            {
                if (usuarioTrabajador != null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdTrabajador == usuarioTrabajador.TrabajadorId)
                        .Select(x => new DDLTrabajador
                        {
                            Id = x.IdTrabajador,
                            Trabajador = x.Nombre
                        }).ToListAsync();

                    unidades = await _context.Unidades.Where(u => u.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                    areas = await _context.Areas.Where(a => a.IdEmpresa == empresa.IdEmpresa).ToListAsync();

                    ViewBag.Trabajadores = trabajadores;
                    ViewBag.Unidades = unidades;
                    ViewBag.areas = areas;

                }
                else
                {
                    return NotFound("Usuario no vinculado a trabajador");
                }
            }

            return View();
        }

        // GET: FactoresRiesgos
        public async Task<IActionResult> Index(int? IdTrabajador, int? IdUnidad, int? IdArea)
        {
            ClaimsPrincipal currentUser = this.User;
            Empresa empresa = null;
            UsuarioTrabajador usuarioTrabajador = null;

            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUserId == null)
                return NotFound();

            usuarioTrabajador = await _context.UsuariosTrabajadores
                .FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));

            if (usuarioTrabajador != null)
            {
                empresa = await _context.Empresas
                    .FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);
            }

            if (currentUser.IsInRole("Admin"))
            {
                if (IdTrabajador != null)
                    return View(await _context.FactoresRiesgos
                        .Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdUnidad != null)
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                List<Trabajador> trabajadores;

                if (IdTrabajador != null)
                {
                    return View(await _context.FactoresRiesgos
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == empresa.IdEmpresa).ToListAsync();

                    if (trabajadores.Count > 0)
                    {
                        List<FactorRiesgo> factoresRiesgos = new List<FactorRiesgo>();

                        foreach (var trabajador in trabajadores)
                        {
                            var factorRiesgo = await _context.FactoresRiesgos
                                .FirstOrDefaultAsync(x => x.IdTrabajador == trabajador.IdTrabajador);

                            if (factorRiesgo != null)
                                factoresRiesgos.Add(factorRiesgo);
                        }

                        if (factoresRiesgos.Count > 0)
                            return View(factoresRiesgos);
                        else
                        {
                            return NotFound();
                        }
                    }
                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .Where(q => q != null && q.IdArea == IdArea)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    //var riesgos = await _context.Unidades
                    //    .Where(u => u.Id == IdUnidad)
                    //    .SelectMany(t => t.Trabajadores)
                    //    .ToListAsync();

                    var riesgos = await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync();

                    return View(riesgos);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.FactoresRiesgos)
                        .ToListAsync());
                }
            }
            else
            {
                var factoresRiesgos = await _context.FactoresRiesgos
                    .Where(r => r.IdTrabajador == usuarioTrabajador.TrabajadorId).ToListAsync();

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
