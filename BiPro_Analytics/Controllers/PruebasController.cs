using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiPro_Analytics.Data;
using BiPro_Analytics.Models;
using System.Security.Claims;
using BiPro_Analytics.Responses;
using BiPro_Analytics.UnParo;
using Microsoft.AspNetCore.Hosting;
using BiPro_Analytics.Models.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BiPro_Analytics.Responses.Pruebas;
using BiPro_Analytics.Models.Catalogs;

namespace BiPro_Analytics.Controllers
{
    public class PruebasController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;
        private readonly IWebHostEnvironment _enviroment;
        private readonly IConfiguration _configuration;

        public PruebasController(BiproAnalyticsDBContext context, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _enviroment = env;
            _configuration = configuration;
        }

        public async Task<IActionResult> PreIndex()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            ViewBag.Unidades = perfilData.DDLUnidades;
            ViewBag.Areas = perfilData.DDLAreas;
            ViewBag.Empresas = perfilData.DDLEmpresas;
            ViewBag.Trabajadores = perfilData.DDLTrabajadores ?? new List<DDLTrabajador>();

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
        // GET: Pruebas

        public async Task<IActionResult> Index(int? IdTrabajador, int? IdUnidad, int? IdArea)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            ViewBag.Unidades = perfilData.DDLUnidades;
            ViewBag.Areas = perfilData.DDLAreas;
            ViewBag.Empresas = perfilData.DDLEmpresas;
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;

            if (currentUser.IsInRole("AdminEmpresa"))
            {
                if (perfilData.IdEmpresa == null)
                    return NotFound("Usuario no asociado a ninguna empresa");
            }

            if (currentUser.IsInRole("Admin"))
            {
                if (IdTrabajador != null)
                    return View(await _context.Pruebas
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());

                if (IdUnidad != null)
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.Pruebas)
                        .Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());


                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.Pruebas)
                        .Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area).ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {

                if (IdTrabajador != null)
                {
                    return View(await _context.Pruebas
                        .Where(x => x.IdTrabajador == IdTrabajador)
                        .Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {

                    List<Prueba> pruebas = new List<Prueba>();

                    pruebas = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(t => t.Pruebas)
                        .Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync();

                    return View(pruebas);

                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea)
                        .SelectMany(r => r.Pruebas)
                        .Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var pruebas = await _context.Trabajadores
                        .Where(u => u.IdUnidad == IdUnidad)
                        .SelectMany(r => r.Pruebas)
                        .Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync();

                    return View(pruebas);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(a => a.IdArea == IdArea)
                        .SelectMany(r => r.Pruebas)
                        .Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());
                }
            }
            else
            {
                var pruebas = await _context.Pruebas
                    .Where(p => p.IdTrabajador == perfilData.IdTrabajador)
                    .Include(p=>p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                    .ToListAsync();

                if (pruebas.Count == 0)
                    return NotFound("Trabajador sin pruebas");

                return View(pruebas);
            }

            return View(await _context.Pruebas
                .Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                .ToListAsync());

        }

        // GET: Pruebas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prueba = await _context.Pruebas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prueba == null)
            {
                return NotFound();
            }

            return View(prueba);
        }

        // GET: Pruebas/Create
        public async Task<IActionResult> Create()
        {
            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;
            var ubicaciones = _context.Ubicacion.ToListAsync();
            ViewBag.Ubicaciones = new SelectList(ubicaciones.Result, "Identificador", "Descripcion");
            var Diagnosticos = await _context.Diagnosticos.Select(d=>d.Diagnostico).ToListAsync();
            ViewBag.Diagnosticos = Diagnosticos;
            ViewBag.Unidades = perfilData.DDLUnidades;
            ViewBag.Areas = perfilData.DDLAreas;

            return View();
        }

        // POST: Pruebas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaDiagnostico,Lugar,DiagnosticoCovid,TipoPrueba,IdTrabajador,UbicacionId,IdUnidad,IdArea")] Prueba prueba)
        {

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (ModelState.IsValid)
            {
                //prueba.Trabajador = _context.Trabajadores.Find(prueba.IdTrabajador);
                prueba.FechaHoraRegistro = DateTime.Now;

                if (currentUser.IsInRole("Trabajador"))
                {
                    prueba.Trabajador = _context.Trabajadores.Find(perfilData.IdTrabajador);
                    prueba.IdTrabajador = (int)perfilData.IdTrabajador;
                }
                else
                {
                    prueba.Trabajador = _context.Trabajadores.Find(prueba.IdTrabajador);
                    prueba.IdTrabajador = prueba.IdTrabajador;
                }
                    

                _context.Add(prueba);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prueba);
        }

        // GET: Pruebas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var prueba = await _context.Pruebas.FindAsync(id);
            if (prueba == null)
            {
                return NotFound();
            }

            ClaimsPrincipal currentUser = this.User;
            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);
            //Para combo Trabajadores
            ViewBag.Trabajadores = perfilData.DDLTrabajadores;
            var ubicaciones = _context.Ubicacion.ToListAsync();
            ViewBag.Ubicaciones = new SelectList(ubicaciones.Result, "Identificador", "Descripcion");
            var Diagnosticos = await _context.Diagnosticos.Select(d => d.Diagnostico).ToListAsync();
            ViewBag.Diagnosticos = Diagnosticos;
            ViewBag.Unidades = perfilData.DDLUnidades;
            ViewBag.Areas = perfilData.DDLAreas;

            return View(prueba);
        }

        // POST: Pruebas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaDiagnostico,Lugar,DiagnosticoCovid,TipoPrueba,IdTrabajador,UbicacionId,IdUnidad,IdArea")] Prueba prueba)
        {
            if (id != prueba.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    prueba.Trabajador = _context.Trabajadores.Find(prueba.IdTrabajador);
                    prueba.FechaHoraRegistro = DateTime.Now;
                    _context.Update(prueba);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PruebaExists(prueba.Id))
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
            return View(prueba);
        }

        // GET: Pruebas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prueba = await _context.Pruebas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prueba == null)
            {
                return NotFound();
            }

            return View(prueba);
        }

        // POST: Pruebas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prueba = await _context.Pruebas.FindAsync(id);
            _context.Pruebas.Remove(prueba);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AdministrarArchivos(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prueba = await _context.Pruebas.FindAsync(id);
            if (prueba == null)
            {
                return NotFound();
            }

            return View(prueba);
        }

        [HttpPost]
        public async Task<IActionResult> SubirArchivos( IFormFile files, int? id )
        {
            TempData["Message"] = "Archivo arriba";
            

            string blobstorageconnection = _configuration.GetValue<string>("blobstorage");

            byte[] dataFiles;

            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(blobstorageconnection);
            // Retrieve a reference to a container.
            BlobContainerClient containerClient =  blobServiceClient.GetBlobContainerClient("biprocovid19");

            // Create the blob client.
            BlobClient cloudBlobClient = containerClient.GetBlobClient(files.FileName);

            try
            {
                await using (var target = new MemoryStream())
                {
                    files.CopyTo(target);
                    dataFiles = target.ToArray();
                    target.Position = 0;
                    await cloudBlobClient.UploadAsync(target, true);
                }

                Archivos archivos = new Archivos
                {
                    NombreArchivo = files.FileName,
                    Url = cloudBlobClient.Uri.AbsoluteUri,
                    IdPrueba = (int)id
                };

                await _context.Archivos.AddAsync(archivos);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }

            return RedirectToAction("Index");
        }

        private bool PruebaExists(int id)
        {
            return _context.Pruebas.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ExportPruebasRealizadasCSv(int? idEmpresa)
        {
            var pruebas = await GetPruebas(idEmpresa);

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,Diagnostico,Lugar,Fecha de diagnostico");

            foreach (var item in pruebas)
            {
                var diagnostico = await _context.Diagnosticos.FirstOrDefaultAsync(t => t.Id == Convert.ToInt32( item.DiagnosticoCovid));

                builder.AppendLine($"{item.Trabajador.Nombre},{diagnostico.Diagnostico}, {item.Lugar},{item.FechaDiagnostico}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "Pruebas" + DateTime.Now + ".csv");
        }

        public async Task<IEnumerable<Prueba>> GetPruebas (int? idEmpresa)
        {
            List<Prueba> pruebas = await _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                .SelectMany(x => x.Pruebas)
                .Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                .ToListAsync();

            var lts = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa);

            return pruebas;
        }

        public JsonResult GetConteTipoPruebasPie (int? idEmpresa)
        {
            List<PieData> pieDatas = new List<PieData>();
            
            string[] lbls = new string[] { "PCR", "Rápida en sangre", "Rápida con pinchazo de dedo", "Rápida nasal", "Tomografía", "Diagnóstico Médico" };


            int[] cnts;

            if (idEmpresa != null)
            {
                cnts = new int[]
                {
                    _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(x => x.Pruebas).Where(p=> p.DiagnosticoCovid == "1").Count(),
                    _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(x => x.Pruebas).Where(p=> p.DiagnosticoCovid == "2").Count(),
                    _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(x => x.Pruebas).Where(p=> p.DiagnosticoCovid == "3").Count(),
                    _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(x => x.Pruebas).Where(p=> p.DiagnosticoCovid == "4").Count(),
                    _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(x => x.Pruebas).Where(p=> p.DiagnosticoCovid == "5").Count(),
                    _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(x => x.Pruebas).Where(p=> p.DiagnosticoCovid == "6").Count()
                };
            }
            else
            {

            }
            
            cnts = new int[]
            {
                _context.Pruebas.Where(p=> p.DiagnosticoCovid == "1").Count(),
                _context.Pruebas.Where(p=> p.DiagnosticoCovid == "2").Count(),
                _context.Pruebas.Where(p=> p.DiagnosticoCovid == "3").Count(),
                _context.Pruebas.Where(p=> p.DiagnosticoCovid == "4").Count(),
                _context.Pruebas.Where(p=> p.DiagnosticoCovid == "5").Count(),
                _context.Pruebas.Where(p=> p.DiagnosticoCovid == "6").Count()
            };

            PieData pieData = new PieData();
            pieData.Labels = lbls;
            pieData.Counts = cnts;

            pieDatas.Add(pieData);

            return Json(pieData);
        }

        public JsonResult ResultadoPruebas(int? IdEmpresa)
        {
            UbicacionPruebas ubicaciones = new UbicacionPruebas();

            List<Prueba> EnCasaAislamiento;
            List<Prueba> Enotracasaaislado;
            List<Prueba> HospitalizadoenpisoNOCOVID;
            List<Prueba> HospitalizadoenpisoCOVID;
            List<Prueba> Enterapiaintensiva;
            List<Prueba> Falleció;

            







            if (IdEmpresa != null)
            {
                EnCasaAislamiento           =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)                    
                                                .SelectMany(x => x.Pruebas).Where(p => p.UbicacionId == 1).ToList();
                Enotracasaaislado           =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)
                                                .SelectMany(x => x.Pruebas).Where(p => p.UbicacionId == 2).ToList();
                HospitalizadoenpisoNOCOVID  =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)
                                                .SelectMany(x => x.Pruebas).Where(p => p.UbicacionId == 3).ToList();
                HospitalizadoenpisoCOVID    =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)  
                                                .SelectMany(x => x.Pruebas).Where(p => p.UbicacionId == 4).ToList();
                Enterapiaintensiva          =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)
                                                .SelectMany(x => x.Pruebas).Where(p => p.UbicacionId == 5).ToList();
                Falleció                    =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)
                                                .SelectMany(x => x.Pruebas).Where(p => p.UbicacionId == 6).ToList();
            }
            else
            {
                EnCasaAislamiento           =         _context.Pruebas.Where(p => p.UbicacionId == 1).ToList();
                Enotracasaaislado           =            _context.Pruebas.Where(p => p.UbicacionId == 2).ToList();
                HospitalizadoenpisoNOCOVID  =   _context.Pruebas.Where(p => p.UbicacionId == 3).ToList();
                HospitalizadoenpisoCOVID    =               _context.Pruebas.Where(p => p.UbicacionId == 4).ToList();
                Enterapiaintensiva          =                _context.Pruebas.Where(p => p.UbicacionId == 5).ToList();
                Falleció                    =         _context.Pruebas.Where(p => p.UbicacionId == 6).ToList();
            }


            int[] cntsPositivos = new int[]
            {
                EnCasaAislamiento.Count(),
                Enotracasaaislado.Count(),
                HospitalizadoenpisoNOCOVID.Count(),
                HospitalizadoenpisoCOVID.Count(),
                Enterapiaintensiva.Count(),
                Falleció.Count()
            };

            ubicaciones.CountsPositivos = cntsPositivos;
            

            return Json(ubicaciones);
        }
    }
}
