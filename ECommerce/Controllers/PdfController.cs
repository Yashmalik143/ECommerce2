using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GenratePdf(int OrderId)
        {
         
            var document = new PdfDocument();
            string Content = $"<h1>Invoice of order<h1>";
            PdfGenerator.AddPdfPages(document, Content,PageSize.A4);
            byte[] response = null;
            using(MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();

            }

            string fileName = "Innvoice.pdf";
            return File(response,"application/pdf", fileName);
           
        }
    }
}
