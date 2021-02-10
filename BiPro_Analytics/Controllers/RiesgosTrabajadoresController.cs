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

namespace BiPro_Analytics.Controllers
{
    public class RiesgosTrabajadoresController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;
        
        public RiesgosTrabajadoresController(BiproAnalyticsDBContext context)
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
                    .Select(x => new DDLTrabajador {
                        Id = x.IdTrabajador, 
                        Trabajador = x.Nombre }).ToList();

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
                        .Select(x => new DDLTrabajador { 
                            Id = x.IdTrabajador, 
                            Trabajador = x.Nombre 
                        }).ToListAsync();

                    ViewBag.Trabajadores = trabajadores;

                    unidades = await _context.Unidades.Where(u => u.IdEmpresa == empresa.IdEmpresa).ToListAsync();
                    ViewBag.Unidades = unidades;

                    if(IdUnidad != null)
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
                        .Select(x => new DDLTrabajador { 
                            Id = x.IdTrabajador, 
                            Trabajador = x.Nombre }).ToListAsync();

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
        // GET: RiesgosTrabajadores
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
            
            if(usuarioTrabajador != null)
            {
                empresa = await _context.Empresas
                    .FirstOrDefaultAsync(e => e.CodigoEmpresa == usuarioTrabajador.CodigoEmpresa);
            }

            if (currentUser.IsInRole("Admin"))
            {
                if (IdTrabajador != null)
                    return View(await _context.RiesgosTrabajadores
                        .Where(x => x.IdTrabajador == IdTrabajador).ToListAsync());

                if (IdUnidad != null)
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.RiesgosTrabajadores)
                        .ToListAsync());

                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.RiesgosTrabajadores)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                List<Trabajador> trabajadores;

                if (IdTrabajador != null)
                {
                    return View(await _context.RiesgosTrabajadores
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {
                    trabajadores = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == empresa.IdEmpresa).ToListAsync();

                    if (trabajadores.Count > 0)
                    {
                        List<RiesgosTrabajador> riesgosTrabajadores = new List<RiesgosTrabajador>();

                        foreach (var trabajador in trabajadores)
                        {
                            var riesgosTrabajador = await _context.RiesgosTrabajadores
                                .FirstOrDefaultAsync(x => x.IdTrabajador == trabajador.IdTrabajador);

                            if (riesgosTrabajador != null)
                                riesgosTrabajadores.Add(riesgosTrabajador);
                        }

                        if (riesgosTrabajadores.Count > 0)
                            return View(riesgosTrabajadores);
                        else
                        {
                            return NotFound("no mames no está lo que buscas morro");
                        }
                    }
                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .Where(q => q != null && q.IdArea == IdArea)
                        .SelectMany(r => r.RiesgosTrabajadores)
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
                        .SelectMany(r => r.RiesgosTrabajadores)
                        .ToListAsync();

                    return View(riesgos);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.RiesgosTrabajadores)
                        .ToListAsync());
                }
            }
            else
            {
                var riesgosTrabajadores = await _context.RiesgosTrabajadores
                    .Where(r => r.IdTrabajador == usuarioTrabajador.TrabajadorId).ToListAsync();

                if (riesgosTrabajadores.Count == 0)
                    return NotFound("Sin encuesta de riesgos");

                return View(riesgosTrabajadores);
            }

            return View(await _context.RiesgosTrabajadores.ToListAsync());

        }

        // GET: RiesgosTrabajadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riesgosTrabajador = await _context.RiesgosTrabajadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (riesgosTrabajador == null)
            {
                return NotFound();
            }

            return View(riesgosTrabajador);
        }

        // GET: RiesgosTrabajadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RiesgosTrabajadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AreaTrabajo,TipoTransporte,CantidadPersonas,DiagnosticoCovid,ContactoCovid,Mas65,Obesidad,Embarazo,Asma,Fumador,CigarrosDia,AniosFumar,Diabetes,Hipertension,EnfermedadCronica,NombreECronica,Rinitis,Sinusitis,CirugiaNasal,Rinofaringea,NombreRinofaringea,Picante,Fiebre,Tos,DolorCabeza,Disnea,Irritabilidad,Diarrea,Escalofrios,Artralgias,Mialgias,Odinofagia,Rinorrea,Polipnea,Vómito,DolorAbdomina,Conjuntivitis,DolorToracico,Anosmia,Disgeusia,Cianosis,Ninguna,TrabajoEnCasa,IdTrabajador")] RiesgosTrabajador riesgosTrabajador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(riesgosTrabajador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(riesgosTrabajador);
        }

        // GET: RiesgosTrabajadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riesgosTrabajador = await _context.RiesgosTrabajadores.FindAsync(id);
            if (riesgosTrabajador == null)
            {
                return NotFound();
            }
            return View(riesgosTrabajador);
        }

        // POST: RiesgosTrabajadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AreaTrabajo,TipoTransporte,CantidadPersonas,DiagnosticoCovid,ContactoCovid,Mas65,Obesidad,Embarazo,Asma,Fumador,CigarrosDia,AniosFumar,Diabetes,Hipertension,EnfermedadCronica,NombreECronica,Rinitis,Sinusitis,CirugiaNasal,Rinofaringea,NombreRinofaringea,Picante,Fiebre,Tos,DolorCabeza,Disnea,Irritabilidad,Diarrea,Escalofrios,Artralgias,Mialgias,Odinofagia,Rinorrea,Polipnea,Vómito,DolorAbdomina,Conjuntivitis,DolorToracico,Anosmia,Disgeusia,Cianosis,Ninguna,TrabajoEnCasa,IdTrabajador")] RiesgosTrabajador riesgosTrabajador)
        {
            if (id != riesgosTrabajador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(riesgosTrabajador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RiesgosTrabajadorExists(riesgosTrabajador.Id))
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
            return View(riesgosTrabajador);
        }

        // GET: RiesgosTrabajadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riesgosTrabajador = await _context.RiesgosTrabajadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (riesgosTrabajador == null)
            {
                return NotFound();
            }

            return View(riesgosTrabajador);
        }

        // POST: RiesgosTrabajadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var riesgosTrabajador = await _context.RiesgosTrabajadores.FindAsync(id);
            _context.RiesgosTrabajadores.Remove(riesgosTrabajador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RiesgosTrabajadorExists(int id)
        {
            return _context.RiesgosTrabajadores.Any(e => e.Id == id);
        }
    }
}
