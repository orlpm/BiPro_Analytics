using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROWAnalytics.Controllers
{
    public class TypeFormController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegistroEmpresa()
        {
            return View();
        }

        public IActionResult Psicosocial()
        {
            return View();
        }

        public IActionResult SaludFisica()
        {
            return View();
        }

        public IActionResult ModuloGastro()
        {
            return View();
        }
    }
}
