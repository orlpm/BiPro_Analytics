using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PROWAnalytics.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PROWAnalytics.UnParo;
using PROWAnalytics.Data;
using PROWAnalytics.EntityFrameworkCore;

namespace PROWAnalytics.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BiproAnalyticsDBContext _context;

        public IServiceProvider _serviceProvider { get; }

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider, BiproAnalyticsDBContext context)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            await CreateRolesAsync(_serviceProvider);

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                {
                    return RedirectToAction("Create", "Empresas");
                }

                var empresa = _context.Empresas.Find(perfilData.IdEmpresa);

                if (!empresa.DescartarUnidades)
                {
                    if (perfilData.DDLUnidades == null)
                    {
                        return RedirectToAction("Index", "Unidades");
                    }
                }

                if (!empresa.DescartarAreas)
                {
                    if (perfilData.DDLAreas == null)
                    {
                        return RedirectToAction("Index", "Areas");
                    }
                }

            }
            if (currentUser.IsInRole("Trabajador"))
            {
                if (perfilData.IdTrabajador == null)
                {
                    return RedirectToAction("Create", "Trabajadores");
                }

                var factoresRiesgo = await _context.FactoresRiesgos.Where(x => x.IdTrabajador == perfilData.IdTrabajador).ToListAsync();
                if (factoresRiesgo.Count > 0)
                    ViewBag.FactoresRiesgo = true;

                var riesgosContagio = await _context.RiesgoContagios.Where(x => x.IdTrabajador == perfilData.IdTrabajador).ToListAsync();
                if (riesgosContagio.Count > 0)
                    ViewBag.RiesgosContagios = true;

                var pruebasExternas = await _context.Pruebas.Where(x => x.IdTrabajador == perfilData.IdTrabajador).ToListAsync();
                if (pruebasExternas.Count > 0)
                    ViewBag.PruebasExternas = true;

                var seguimientoCovid = await _context.SeguimientosCovid.Where(x => x.IdTrabajador == perfilData.IdTrabajador).ToListAsync();
                if (seguimientoCovid.Count > 0)
                    ViewBag.SeguimientoCovid = true;

                var reincorporados = await _context.Reincorporados.Where(x => x.IdTrabajador == perfilData.IdTrabajador).ToListAsync();
                if (reincorporados.Count > 0)
                    ViewBag.Reincorporados = true;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Admin,AdminEmpresa")]
        private async Task CreateRolesAsync (IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] rolesName = { "Admin", "AdminEmpresa", "Trabajador" };

            foreach (var item in rolesName)
            {
                var roleExist = await roleManager.RoleExistsAsync(item);
                if(!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(item));
                }
            }
            //Guid userId = new Guid();
            //var user = await userManager.FindByIdAsync(userId.ToString());
            //await userManager.AddToRoleAsync(user, "Admin");
        }

        public async Task<IActionResult> CheckRelAreasUnidades (string controller)
        {
            ClaimsPrincipal currentUser = this.User;
            
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (perfilData.IdEmpresa == null)
            {
                return RedirectToAction("Create", "Empresas");
            }

            var empresa = _context.Empresas.Find(perfilData.IdEmpresa);

            if (!empresa.DescartarUnidades)
            {
                if (perfilData.DDLUnidades == null)
                {
                    return RedirectToAction("Index", "Unidades");
                }
            }

            if (!empresa.DescartarAreas)
            {
                if (perfilData.DDLAreas == null)
                {
                    return RedirectToAction("Index", "Areas");
                }
            }

            return RedirectToAction("Index", controller);
        }
    }
}