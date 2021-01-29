using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;

namespace BiPro_Analytics.Controllers
{
    public class ChartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerarPDF(string html)
        {
            html = html.Replace("StrTag", "<").Replace("EndTag", ">");

            HtmlToPdf oHTMLToPDF = new HtmlToPdf();
            PdfDocument oPdfDocument = oHTMLToPDF.ConvertHtmlString(html);
            byte[] pdf = oPdfDocument.Save();

            return File(
                    pdf,
                    "application/pdf",
                    "ReporteBiPro.pdf"
                );
        }
    }
}
