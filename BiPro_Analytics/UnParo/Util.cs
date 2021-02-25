using BiPro_Analytics.Data;
using BiPro_Analytics.Models;
using BiPro_Analytics.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BiPro_Analytics.UnParo
{
    public class Util
    {
        private readonly BiproAnalyticsDBContext _context;
        private readonly ClaimsPrincipal _currentUser;

        public Util(BiproAnalyticsDBContext context)
        {
            _context = context;
        }
        public async Task<PerfilData> DatosUserAsync(ClaimsPrincipal currentUser)
        {
            PerfilData perfilData = new PerfilData();

            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            UsuarioEmpresa usuarioEmpresa = null;
            UsuarioTrabajador usuarioTrabajador = null;

            if (currentUser.IsInRole("Admin"))
            {
                List<DDLUnidad> unidades = await _context.Unidades
                    .Select(x => new DDLUnidad { Id = x.Id, Unidad = x.Name }).ToListAsync();
                if (unidades.Count > 0)
                    perfilData.DDLUnidades = unidades;

                List<DDLArea> areas = await _context.Areas
                    .Select(x => new DDLArea { Id = x.Id, Area = x.Name }).ToListAsync();
                if (areas.Count > 0)
                    perfilData.DDLAreas = areas;

                List<DDLEmpresa> empresas = await _context.Empresas
                    .Select(x => new DDLEmpresa { Id = x.IdEmpresa, Empresa = x.Nombre }).ToListAsync();
                if (empresas.Count > 0)
                    perfilData.DDLEmpresas = empresas;

                List<DDLTrabajador> trabajadores =  _context.Trabajadores
                    .Select(x => new DDLTrabajador { Id = x.IdTrabajador, Trabajador = x.Nombre }).ToList();
                if (trabajadores.Count > 0)
                    perfilData.DDLTrabajadores = trabajadores;

            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {
                usuarioEmpresa = await _context.UsuariosEmpresas.FirstOrDefaultAsync(u => u.IdUsuario == Guid.Parse(currentUserId));

                List<DDLUnidad> unidades = await _context.Unidades.Where(u => u.IdEmpresa == usuarioEmpresa.IdEmpresa)
                    .Select(x => new DDLUnidad { Id = x.Id, Unidad = x.Name }).ToListAsync();
                if (unidades.Count > 0)
                    perfilData.DDLUnidades = unidades;

                List<DDLArea> areas = await _context.Areas.Where(a => a.IdEmpresa == usuarioEmpresa.IdEmpresa)
                    .Select(x => new DDLArea { Id = x.Id, Area = x.Name }).ToListAsync();
                if (areas.Count > 0)
                    perfilData.DDLAreas = areas;

                List<DDLEmpresa> empresas = await _context.Empresas.Where(e => e.IdEmpresa == usuarioEmpresa.IdEmpresa)
                    .Select(x => new DDLEmpresa { Id = x.IdEmpresa, Empresa = x.Nombre }).ToListAsync();
                if (empresas.Count > 0)
                    perfilData.DDLEmpresas = empresas;

                List<DDLTrabajador> trabajadores = await _context.Trabajadores
                    .Where(t => t.IdEmpresa == usuarioEmpresa.IdEmpresa)
                    .Select(x => new DDLTrabajador { Id = x.IdTrabajador, Trabajador = x.Nombre })
                    .ToListAsync();
                if (trabajadores.Count > 0)
                    perfilData.DDLTrabajadores = trabajadores;

            }
            if (currentUser.IsInRole("Trabajador"))
            {
                usuarioTrabajador = await _context.UsuariosTrabajadores.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(currentUserId));
                List<DDLTrabajador> trabajadores = await _context.Trabajadores
                    .Where(t => t.IdTrabajador == usuarioTrabajador.TrabajadorId)
                    .Select(x => new DDLTrabajador { Id = x.IdTrabajador, Trabajador = x.Nombre })
                    .ToListAsync();
                if (trabajadores.Count > 0)
                    perfilData.DDLTrabajadores = trabajadores;

            }

            return perfilData;
        }
    }
}
