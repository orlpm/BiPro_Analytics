using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiPro_Analytics.Data;
using BiPro_Analytics.Models;

namespace BiPro_Analytics.Controllers
{
    public class RiesgosTrabajadoresController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public RiesgosTrabajadoresController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        // GET: RiesgosTrabajadores
        public async Task<IActionResult> Index()
        {
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
