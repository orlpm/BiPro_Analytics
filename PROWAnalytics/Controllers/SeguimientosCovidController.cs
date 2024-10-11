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
using PROWAnalytics.UnParo;
using PROWAnalytics.Responses;
using System.Text;

namespace PROWAnalytics.Controllers
{
    public class SeguimientosCovidController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public SeguimientosCovidController(BiproAnalyticsDBContext context)
        {
            _context = context;
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
            ViewBag.Trabajadores = perfilData.DDLTrabajadores != null ? perfilData.DDLTrabajadores : new List<DDLTrabajador>();

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                {
                    return NotFound("Datos de empresa no encontrados");
                }
                ViewBag.IdEmpresa = perfilData.IdEmpresa;
            }

            return View();
        }

        // GET: SeguimientosCovid
        public async Task<IActionResult> Index()
        {

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                    return NotFound("Usuario no asociado a ninguna empresa");
            }


            if (currentUser.IsInRole("AdminEmpresa") || currentUser.IsInRole("Trabajador"))
                return View(await _context.Trabajadores.Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                    .SelectMany(t => t.SeguimientosCovid)
                    .Include(s => s.Trabajador).Include(s => s.UbicacionActual).Include(s => s.SintomaCovid)
                    .ToListAsync());
            
            if (currentUser.IsInRole("Admin"))
                return View(await _context.SeguimientosCovid
                    .Include(s => s.Trabajador).Include(s => s.UbicacionActual).Include(s => s.SintomaCovid)
                    .ToListAsync());
            else
                return View(Forbid());
        }

        // GET: SeguimientosCovid/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid
                .Include(s => s.Trabajador).Include(s => s.UbicacionActual).Include(s => s.SintomaCovid)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Create
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            ViewData["IdUbicacion"] = new SelectList(_context.Ubicacion, "Identificador", "Descripcion");
            ViewData["IdSintomas"] = new SelectList(_context.SintomasCovid, "IdSintoma", "Sintoma");

            if (currentUser.IsInRole("Admin"))
                ViewData["IdTrabajador"] = new SelectList(_context.Trabajadores, "IdTrabajador", "Nombre");

            if (currentUser.IsInRole("AdminEmpresa"))
                ViewData["IdTrabajador"] = new SelectList(_context.Trabajadores.Where(t => t.IdEmpresa == perfilData.IdEmpresa), "IdTrabajador", "Nombre");

            return View();
        }

        // POST: SeguimientosCovid/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Estado,IdUbicacion,FechaSeguimiento,FechaHoraRegistro,IdTrabajador,IdSintoma")] SeguimientoCovid seguimientoCovid)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (currentUser.IsInRole("Trabajador"))
                seguimientoCovid.IdTrabajador = perfilData.IdTrabajador;

            if (ModelState.IsValid)
            {
                _context.Add(seguimientoCovid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (currentUser.IsInRole("Admin"))
                ViewData["IdTrabajador"] = new SelectList(_context.Trabajadores, "IdTrabajador", "Nombre", seguimientoCovid.IdTrabajador);

            if (currentUser.IsInRole("AdminEmpresa"))
                ViewData["IdTrabajador"] = new SelectList(_context.Trabajadores.Where(t => t.IdEmpresa == perfilData.IdEmpresa), "IdTrabajador", "Nombre", seguimientoCovid.IdTrabajador);

            return View(seguimientoCovid);
        }

        // GET: SeguimientosCovid/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (id == null)
            {
                return NotFound();
            }

            var seguimientoCovid = await _context.SeguimientosCovid.FindAsync(id);
            
            if (seguimientoCovid == null)
            {
                return NotFound();
            }

            ViewData["IdUbicacion"] = new SelectList(_context.Ubicacion, "Identificador", "Descripcion");
            ViewData["IdSintomas"] = new SelectList(_context.SintomasCovid, "IdSintoma", "Sintoma");

            if (currentUser.IsInRole("Admin"))
                ViewData["IdTrabajador"] = new SelectList(_context.Trabajadores, "IdTrabajador", "Nombre");

            if (currentUser.IsInRole("AdminEmpresa"))
                ViewData["IdTrabajador"] = new SelectList(_context.Trabajadores.Where(t => t.IdEmpresa == perfilData.IdEmpresa), "IdTrabajador", "Nombre");
            return View(seguimientoCovid);
        }

        // POST: SeguimientosCovid/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Estado,IdUbicacion,FechaSeguimiento,FechaHoraRegistro,IdTrabajador,IdSintoma")] SeguimientoCovid seguimientoCovid)
        {
            if (id != seguimientoCovid.Id)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (currentUser.IsInRole("Trabajador"))
                seguimientoCovid.IdTrabajador = perfilData.IdTrabajador;

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["IdTrabajador"] = new SelectList(_context.Trabajadores, "IdTrabajador", "CP", seguimientoCovid.IdTrabajador);
            ViewData["IdSintoma"] = new SelectList(_context.Trabajadores, "Id", "CP", seguimientoCovid.IdTrabajador);

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
                .Include(s => s.Trabajador).Include(s => s.UbicacionActual).Include(s => s.SintomaCovid)
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

        public JsonResult GetSintomasPie(int? idEmpresa)
        {
            List<PieData> pieDatas = new List<PieData>();
            var sintomas = _context.SintomasCovid.Select(s=>s.Sintoma).ToList();
            var Idsintomas = _context.SintomasCovid.Select(s => s.IdSintoma).ToList();

            string[] lbls = sintomas.ToArray();


            int[] cnts;
            List<int> cnts1 = new List<int>();

            if (idEmpresa != null)
            {
                foreach (var item in Idsintomas)
                {
                    cnts1.Add(_context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                        .SelectMany(x => x.SeguimientosCovid).Where(p => p.IdSintoma == item).Count());
                }
            }
            
            PieData pieData = new PieData();
            pieData.Labels = lbls;
            pieData.Counts = cnts1.ToArray();

            pieDatas.Add(pieData);

            return Json(pieData);
        }

        public JsonResult GetUbicacionesPie(int? idEmpresa)
        {
            List<PieData> pieDatas = new List<PieData>();
            var ubicaciones = _context.Ubicacion.Select(s => s.Descripcion).ToList();
            var IdUbicaciones = _context.Ubicacion.Select(s => s.Identificador).ToList();

            string[] lbls = ubicaciones.ToArray();

            List<int> cnts1 = new List<int>();

            if (idEmpresa != null)
            {
                foreach (var item in IdUbicaciones)
                {
                    cnts1.Add(_context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                        .SelectMany(x => x.SeguimientosCovid).Where(p => p.IdUbicacion == item).Count());
                }
            }

            PieData pieData = new PieData();
            pieData.Labels = lbls;
            pieData.Counts = cnts1.ToArray();

            pieDatas.Add(pieData);

            return Json(pieData);
        }

        public async Task<IActionResult> ExportTrabajadoresSeguimientoGralCSV(int idEmpresa)
        {
            var seguimientosCovid = await _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(t => t.SeguimientosCovid)
                    .Include(s => s.Trabajador).Include(s => s.UbicacionActual).Include(s => s.SintomaCovid)
                    .ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,Sintoma,Ubicación,");

            foreach (var item in seguimientosCovid)
            {
                builder.AppendLine($"{item.Trabajador.Nombre},{item.SintomaCovid.Sintoma},{item.UbicacionActual.Descripcion}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "TrabajadoresSeguimientoGralCSV" + DateTime.Now + ".csv");
        }
    }
}
