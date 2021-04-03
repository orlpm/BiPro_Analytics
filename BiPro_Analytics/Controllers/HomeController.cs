﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BiPro_Analytics.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BiPro_Analytics.UnParo;
using BiPro_Analytics.Data;

namespace BiPro_Analytics.Controllers
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
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            await CreateRolesAsync(_serviceProvider);

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                {
                    return RedirectToAction("Create", "Empresas");
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Admin")]
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
    }
}