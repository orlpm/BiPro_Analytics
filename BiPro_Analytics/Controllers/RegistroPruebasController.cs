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
    public class RegistroPruebasController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public RegistroPruebasController(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        // GET: RegistroPruebas
        public async Task<IActionResult> Index()
        {
            return View(await _context.RegistroPruebas.ToListAsync());
        }

        // GET: RegistroPruebas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroPrueba = await _context.RegistroPruebas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registroPrueba == null)
            {
                return NotFound();
            }

            return View(registroPrueba);
        }

        // GET: RegistroPruebas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegistroPruebas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Temperatura,PorcentajeO2,TipoSangre,APOlfativa,APGustativa,Mas15cm,Menos15cm,PIE3,PIE4,PIE5,Discriminacion,Total,Diagnostico,ResultadoIgM,ResultadoIgG,ResultadoPCR,IdTrabajador")] RegistroPrueba registroPrueba)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registroPrueba);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registroPrueba);
        }

        // GET: RegistroPruebas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroPrueba = await _context.RegistroPruebas.FindAsync(id);
            if (registroPrueba == null)
            {
                return NotFound();
            }
            return View(registroPrueba);
        }

        // POST: RegistroPruebas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Temperatura,PorcentajeO2,TipoSangre,APOlfativa,APGustativa,Mas15cm,Menos15cm,PIE3,PIE4,PIE5,Discriminacion,Total,Diagnostico,ResultadoIgM,ResultadoIgG,ResultadoPCR,IdTrabajador")] RegistroPrueba registroPrueba)
        {
            if (id != registroPrueba.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registroPrueba);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroPruebaExists(registroPrueba.Id))
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
            return View(registroPrueba);
        }

        // GET: RegistroPruebas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroPrueba = await _context.RegistroPruebas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registroPrueba == null)
            {
                return NotFound();
            }

            return View(registroPrueba);
        }

        // POST: RegistroPruebas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registroPrueba = await _context.RegistroPruebas.FindAsync(id);
            _context.RegistroPruebas.Remove(registroPrueba);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistroPruebaExists(int id)
        {
            return _context.RegistroPruebas.Any(e => e.Id == id);
        }
    }
}
