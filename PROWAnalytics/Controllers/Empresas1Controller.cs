using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROWAnalytics.Data;
using PROWAnalytics.Models;

namespace PROWAnalytics.Controllers
{
    public class Empresas1Controller : Controller
    {
        private readonly BiproAnalyticsDBContext _context;

        public Empresas1Controller(BiproAnalyticsDBContext context)
        {
            _context = context;
        }

        // GET: Empresas1
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empresas.ToListAsync());
        }

        // GET: Empresas1/Details/5
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

        // GET: Empresas1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpresa,Nombre,RazonSocial,RFC,ActividadPrincipal,Sector,Calle,NumeroExt,NumeroInt,Colonia,Ciudad,Municipio,Estado,CP,Aministrador,Puesto,Telefono,Correo,CantidadEmpleados,SueldoMinimo,SueldoMaximo,SueldoPromedio,NumeroSucursales,Comedor,TransporteTrabajadores,ServicioMedico,SGMM,TrabajadoresConSGMM,NombreAseguradora,NombreAgenteSeguros,HorasLaborales,DiasLaborales,FechaRegistro,CodigoEmpresa,DescartarUnidades,DescartarAreas")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Empresas1/Edit/5
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

        // POST: Empresas1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpresa,Nombre,RazonSocial,RFC,ActividadPrincipal,Sector,Calle,NumeroExt,NumeroInt,Colonia,Ciudad,Municipio,Estado,CP,Aministrador,Puesto,Telefono,Correo,CantidadEmpleados,SueldoMinimo,SueldoMaximo,SueldoPromedio,NumeroSucursales,Comedor,TransporteTrabajadores,ServicioMedico,SGMM,TrabajadoresConSGMM,NombreAseguradora,NombreAgenteSeguros,HorasLaborales,DiasLaborales,FechaRegistro,CodigoEmpresa,DescartarUnidades,DescartarAreas")] Empresa empresa)
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

        // GET: Empresas1/Delete/5
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

        // POST: Empresas1/Delete/5
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
