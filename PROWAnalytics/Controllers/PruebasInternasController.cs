using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROWAnalytics.Data;
using PROWAnalytics.Models;
using System.Security.Claims;
using PROWAnalytics.Responses;
using PROWAnalytics.UnParo;
using Microsoft.AspNetCore.Hosting;
using PROWAnalytics.Models.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PROWAnalytics.Responses.Pruebas;
using PROWAnalytics.Models.Catalogs;

namespace PROWAnalytics.Controllers
{
    public class PruebasInternasController : Controller
    {
        private readonly BiproAnalyticsDBContext _context;
        private readonly IWebHostEnvironment _enviroment;
        private readonly IConfiguration _configuration;

        public PruebasInternasController(BiproAnalyticsDBContext context, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _enviroment = env;
            _configuration = configuration;
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
                    return View(await _context.PruebasInternas
                        .Where(x => x.IdTrabajador == IdTrabajador).Include(p => p.Trabajador)
                        .Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());

                if (IdUnidad != null)
                    return View(await _context.Unidades
                        .Where(u => u.Id == IdUnidad)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.PruebasInternas).Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());


                if (IdUnidad != null && IdArea != null)
                    return View(await _context.Areas
                        .Where(a => a.Id == IdArea)
                        .SelectMany(t => t.Trabajadores)
                        .SelectMany(r => r.PruebasInternas).Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());
            }
            else if (currentUser.IsInRole("AdminEmpresa"))
            {

                if (IdTrabajador != null)
                {
                    return View(await _context.PruebasInternas
                        .Where(x => x.IdTrabajador == IdTrabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());
                }
                else if (IdUnidad == null && IdArea == null)
                {

                    List<PruebaInterna> pruebasInternas = new List<PruebaInterna>();

                    pruebasInternas = await _context.Trabajadores
                        .Where(t => t.IdEmpresa == perfilData.IdEmpresa)
                        .SelectMany(t => t.PruebasInternas).Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync();

                    return View(pruebasInternas);

                }
                else if (IdUnidad != null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(t => t.IdUnidad == IdUnidad && t.IdArea == IdArea)
                        .SelectMany(r => r.PruebasInternas).Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());
                }
                else if (IdUnidad != null && IdArea == null)
                {
                    var pruebasInternas = await _context.Trabajadores
                        .Where(u => u.IdUnidad == IdUnidad)
                        .SelectMany(r => r.PruebasInternas).Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync();

                    return View(pruebasInternas);
                }
                else if (IdUnidad == null && IdArea != null)
                {
                    return View(await _context.Trabajadores
                        .Where(a => a.IdArea == IdArea)
                        .SelectMany(r => r.PruebasInternas).Include(p => p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                        .ToListAsync());
                }
            }
            else
            {
                var pruebasInternas = await _context.PruebasInternas
                    .Where(p => p.IdTrabajador == perfilData.IdTrabajador)
                    .Include(p=>p.Trabajador).Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                    .ToListAsync();

                if (pruebasInternas.Count == 0)
                    return NotFound("Trabajador sin pruebas");

                return View(pruebasInternas);
            }

            return View(await _context.PruebasInternas.Include(p => p.Trabajador)
                .Include(p => p.UbicacionActual).Include(p => p.Unidad).Include(p => p.Area)
                .ToListAsync());

        }

        // GET: Pruebas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebaInterna = await _context.PruebasInternas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pruebaInterna == null)
            {
                return NotFound();
            }

            return View(pruebaInterna);
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
            var Diagnosticos = await _context.Diagnosticos.Select(d => d.Diagnostico).ToListAsync();
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
        public async Task<IActionResult> Create([Bind("Id,FechaDiagnostico,Lugar,Resultado,TipoPrueba,IdTrabajador,UbicacionId,IdUnidad,IdArea")] PruebaInterna pruebaInterna)
        {

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Util util = new Util(_context);
            PerfilData perfilData = await util.DatosUserAsync(currentUser);

            if (ModelState.IsValid)
            {
                //prueba.Trabajador = _context.Trabajadores.Find(prueba.IdTrabajador);
                pruebaInterna.FechaHoraRegistro = DateTime.Now;

                if (currentUser.IsInRole("Trabajador"))
                {
                    pruebaInterna.Trabajador = _context.Trabajadores.Find(perfilData.IdTrabajador);
                    pruebaInterna.IdTrabajador = (int)perfilData.IdTrabajador;
                }
                else
                {
                    pruebaInterna.Trabajador = _context.Trabajadores.Find(pruebaInterna.IdTrabajador);
                    pruebaInterna.IdTrabajador = pruebaInterna.IdTrabajador;
                }
                    

                _context.Add(pruebaInterna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pruebaInterna);
        }

        // GET: Pruebas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebaInterna = await _context.PruebasInternas.FindAsync(id);
            if (pruebaInterna == null)
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

            return View(pruebaInterna);
        }

        // POST: Pruebas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaDiagnostico,Lugar,Resultado,TipoPrueba,IdTrabajador,UbicacionId,IdUnidad,IdArea")] PruebaInterna pruebaInterna)
        {
            if (id != pruebaInterna.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    pruebaInterna.Trabajador = _context.Trabajadores.Find(pruebaInterna.IdTrabajador);
                    pruebaInterna.FechaHoraRegistro = DateTime.Now;
                    _context.Update(pruebaInterna);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PruebaExists(pruebaInterna.Id))
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
            return View(pruebaInterna);
        }

        // GET: Pruebas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebaInterna = await _context.PruebasInternas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pruebaInterna == null)
            {
                return NotFound();
            }

            return View(pruebaInterna);
        }

        // POST: Pruebas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pruebaInterna = await _context.PruebasInternas.FindAsync(id);
            _context.PruebasInternas.Remove(pruebaInterna);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AdministrarArchivos(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pruebaInterna = await _context.PruebasInternas.FindAsync(id);
            if (pruebaInterna == null)
            {
                return NotFound();
            }

            return View(pruebaInterna);
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
            return _context.PruebasInternas.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ExportPruebasRealizadasCSv(int? idEmpresa)
        {
            var pruebasInternas = await GetPruebasInternas(idEmpresa);

            var builder = new StringBuilder();
            builder.AppendLine("Nombre,Resultado,Lugar,Fecha de diagnostico");

            foreach (var item in pruebasInternas)
            {
                var nombreTrabajador = await _context.Trabajadores.FirstOrDefaultAsync(t => t.IdTrabajador == item.IdTrabajador);

                builder.AppendLine($"{nombreTrabajador.Nombre},{item.Resultado}, {item.Lugar},{item.FechaDiagnostico}");
            }

            return File(Encoding.UTF32.GetBytes(builder.ToString()), "text/csv", "PruebasInternas" + DateTime.Now + ".csv");
        }

        public async Task<IEnumerable<PruebaInterna>> GetPruebasInternas (int? idEmpresa)
        {
            List<PruebaInterna> pruebasInternas = await _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                .SelectMany(x => x.PruebasInternas).ToListAsync();

            var lts = _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa);

            return pruebasInternas;
        }

        public JsonResult GetConteoResultadoPruebasInternasPie (int? idEmpresa)
        {
            List<PieData> pieDatas = new List<PieData>();
            
            string[] lbls = new string[] { "Negativos", "Positivos"};

            int[] cnts;

            if (idEmpresa != null)
            {
                cnts = new int[]
                {
                    _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(x => x.PruebasInternas).Where(p=> p.Resultado == "Negativo").Count(),
                    _context.Trabajadores.Where(t => t.IdEmpresa == idEmpresa)
                    .SelectMany(x => x.PruebasInternas).Where(p=> p.Resultado == "Positivo").Count()
                };
            }
            else
            {
                cnts = new int[]
                {
                    _context.PruebasInternas.Where(p=> p.Resultado == "Negativo").Count(),
                    _context.PruebasInternas.Where(p=> p.Resultado == "Positivo").Count(),
                };
            }

            PieData pieData = new PieData();
            pieData.Labels = lbls;
            pieData.Counts = cnts;

            pieDatas.Add(pieData);

            return Json(pieData);
        }

        public JsonResult ResultadoPruebasInternas(int? IdEmpresa)
        {
            PositivosSospechosos resultadoPruebas = new PositivosSospechosos();

            List<PruebaInterna> PCR;
            List<PruebaInterna> Rapidaensangre;
            List<PruebaInterna> Rapidaconpinchazodededo;
            List<PruebaInterna> Rapidanasal;
            List<PruebaInterna> Tomografia;
            List<PruebaInterna> DiagnosticoMedico;

            if (IdEmpresa != null)
            {
                PCR                     =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)                    
                                                .SelectMany(x => x.PruebasInternas).Where(p => p.TipoPrueba == "1").ToList();
                Rapidaensangre          =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)
                                                .SelectMany(x => x.PruebasInternas).Where(p => p.TipoPrueba == "2").ToList();
                Rapidaconpinchazodededo =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)
                                                .SelectMany(x => x.PruebasInternas).Where(p => p.TipoPrueba == "3").ToList();
                Rapidanasal             =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)  
                                                .SelectMany(x => x.PruebasInternas).Where(p => p.TipoPrueba == "4").ToList();
                Tomografia              =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)
                                                .SelectMany(x => x.PruebasInternas).Where(p => p.TipoPrueba == "5").ToList();
                DiagnosticoMedico       =   _context.Trabajadores.Where(t => t.IdEmpresa == IdEmpresa)
                                                .SelectMany(x => x.PruebasInternas).Where(p => p.TipoPrueba == "6").ToList();
            }
            else
            {
                PCR =                       _context.PruebasInternas.Where(p => p.TipoPrueba == "1").ToList();
                Rapidaensangre =            _context.PruebasInternas.Where(p => p.TipoPrueba == "2").ToList();
                Rapidaconpinchazodededo =   _context.PruebasInternas.Where(p => p.TipoPrueba == "3").ToList();
                Rapidanasal =               _context.PruebasInternas.Where(p => p.TipoPrueba == "4").ToList();
                Tomografia =                _context.PruebasInternas.Where(p => p.TipoPrueba == "5").ToList();
                DiagnosticoMedico =         _context.PruebasInternas.Where(p => p.TipoPrueba == "6").ToList();
            }


            int[] cntsPositivos = new int[]
            {
                PCR
                .Where(p=>p.Resultado == "Positivo").Count(),
                Rapidaensangre
                .Where(p=>p.Resultado == "Positivo").Count(),
                Rapidaconpinchazodededo
                .Where(p=>p.Resultado == "Positivo").Count(),
                Rapidanasal
                .Where(p=>p.Resultado == "Positivo").Count(),
                Tomografia
                .Where(p=>p.Resultado == "Positivo").Count(),
                DiagnosticoMedico
                .Where(p=>p.Resultado == "Positivo").Count(),
            };

            int[] cntsNegrativos = new int[]
            {
                PCR
                .Where(p=>p.Resultado == "Negativo").Count(),
                Rapidaensangre
                .Where(p=>p.Resultado == "Negativo").Count(),
                Rapidaconpinchazodededo
                .Where(p=>p.Resultado == "Negativo").Count(),
                Rapidanasal
                .Where(p=>p.Resultado == "Negativo").Count(),
                Tomografia
                .Where(p=>p.Resultado == "Negativo").Count(),
                DiagnosticoMedico
                .Where(p=>p.Resultado == "Negativo").Count(),
            };

            int[] cntsSospechosos = new int[]
            {
                PCR
                .Where(p=>p.Resultado == "").Count(),
                Rapidaensangre
                .Where(p=>p.Resultado == "").Count(),
                Rapidaconpinchazodededo
                .Where(p=>p.Resultado == "").Count(),
                Rapidanasal
                .Where(p=>p.Resultado == "").Count(),
                Tomografia
                .Where(p=>p.Resultado == "").Count(),
                DiagnosticoMedico
                .Where(p=>p.Resultado == "").Count(),
            };

            resultadoPruebas.CountsPositivos = cntsPositivos;
            resultadoPruebas.CountsNegativos = cntsNegrativos;
            resultadoPruebas.CountsSospechosos = cntsSospechosos;

            return Json(resultadoPruebas);
        }
    }
}
