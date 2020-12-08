using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiPro_Analytics.Controllers
{
    [Authorize(Roles = "Admin, AdminEmpresa")]
    public class TableroController : Controller
    {
        // GET: TableroController
        public IActionResult Charts()
        {
            return View();
        }

        // GET: TableroController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TableroController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TableroController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TableroController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TableroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TableroController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TableroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
