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
    public class SeguimientosCovidController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public SeguimientosCovidController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PreIndex(int? IdUnidad, int? IdArea)
        {
            ClaimsPrincipal currentUser = this.User;
            Empresa empresa = null;
            UsuarioTrabajador usuarioTrabajador = null;
            List<DDLTrabajador> trabajadores = null;
            List<Unidad> unidades = null;
            List<Area> areas = null;

            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUserId != null)
                usuarioTrabajador = await _context.UsuariosTrabajadores
                    .FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));

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
                if (empresa != null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == empresa.IdEmpresa)
                        .Select(x => new DDLTrabajador
                        {
                            Id = x.IdTrabajador,
                            Trabajador = x.Nombre
                        }).ToListAsync();

                    ViewBag.Trabajadores = trabajadores;

                    unidades = await _context.Unidades.Where(u => u.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                    ViewBag.Unidades = unidades;

                    if (IdUnidad != null)
                    {
                        ViewBag.Unidad = IdUnidad;
                    }

                    areas = await _context.Areas.Where(a => a.IdEmpresa == empresa.IdEmpresa).ToListAsync();
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
        // GET: SeguimientosCovid
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
            empresa = await _context.Empresas
                .FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);

            if (currentUser.IsInRole("Admin"))
            {
                if (IdTrabajador != null)
                    return View(await _context.SeguimientosCovid
                        .Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdUnidad != null)
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.SeguimientosCovid)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                List<Trabajador> trabajadores;

                if (IdTrabajador != null)
                {
                    return View(await _context.SeguimientosCovid
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == empresa.IdEmpresa).ToListAsync();

                    if (trabajadores.Count > 0)
                    {
                        List<SeguimientoCovid> seguimientos = new List<SeguimientoCovid>();

                        foreach (var trabajador in trabajadores)
                        {
                            var seguimientoCovid = await _context.SeguimientosCovid
                                .FirstOrDefaultAsync(x => x.IdTrabajador == trabajador.IdTrabajador);

                            if (seguimientoCovid != null)
                                seguimientos.Add(seguimientoCovid);
                        }

                        if (seguimientos.Count > 0)
                            return View(seguimientos);
                        else
                        {
                            return NotFound("Sin resultados");
                        }
                    }
                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .Where(q => q != null && q.IdArea == IdArea)
                        .SelectMany(r => r.RegistroPruebas)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var vpruebas = await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .ToListAsync();

                    var pruebas = await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.RegistroPruebas)
                        .ToListAsync();

                    return View(pruebas);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.RegistroPruebas)
                        .ToListAsync());
                }
            }
            else
            {
                var registroPruebas = await _context.RegistroPruebas
                    .Where(r => r.IdTrabajador == usuarioTrabajador.TrabajadorId).ToListAsync();

                if (registroPruebas.Count == 0)
                    return NotFound("Sin registros de pruebas");

                return View(registroPruebas);
            }

            return View(await _context.RegistroPruebas.ToListAsync());

        }

        // GET: SeguimientosCovid/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Create
        public IActionResult Create()
        {
            //Para combo Trabajadores
            List<DDLTrabajador> trabajadores = null;
            trabajadores = _context.Trabajadores
                    .Select(x => new DDLTrabajador
                    {
                        Id = x.IdTrabajador,
                        Trabajador = x.Nombre
                    }).ToList();

            return View();
        }

        // POST: SeguimientosCovid/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EstatusPaciente,SintomasMayores,SintomasMenores,EstatusEnCasa,EstatusEnHospital,FechaSeguimiento,IdTrabajador")] SeguimientoCovid seguimientoCovid)
        {
            if (ModelState.IsValid)
            {
                seguimientoCovid.Trabajador = _context.Trabajadores.Find(seguimientoCovid.IdTrabajador);
                _context.Add(seguimientoCovid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid.FindAsync(id);
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            //Para combo Trabajadores
            List<DDLTrabajador> trabajadores = null;
            trabajadores = _context.Trabajadores
                    .Select(x => new DDLTrabajador
                    {
                        Id = x.IdTrabajador,
                        Trabajador = x.Nombre
                    }).ToList();

            return View(seguimientoCovid);
        }

        // POST: SeguimientosCovid/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EstatusPaciente,SintomasMayores,SintomasMenores,EstatusEnCasa,EstatusEnHospital,FechaSeguimiento,IdTrabajador")] SeguimientoCovid seguimientoCovid)
        {
            if (id != seguimientoCovid.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    seguimientoCovid.Trabajador = _context.Trabajadores.Find(seguimientoCovid.IdTrabajador);
                    _context.Update(seguimientoCovid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeguimientoCovidExists(seguimientoCovid.Id))
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
            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            return View(seguimientoCovid);
        }

        // POST: SeguimientosCovid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seguimientoCovid = await _context.SeguimientosCovid.FindAsync(id);
            _context.SeguimientosCovid.Remove(seguimientoCovid);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeguimientoCovidExists(int id)
        {
            return _context.SeguimientosCovid.Any(e => e.Id == id);
        }
    }
}
