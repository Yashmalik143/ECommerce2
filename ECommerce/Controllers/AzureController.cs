using BusinessLayer.Repository;
using DataAcessLayer.FormFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureController : ControllerBase
    {
         private readonly IAzureLogic _Azure;

        public AzureController(IAzureLogic fileManagerLogic)
        {
            _Azure = fileManagerLogic;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]FileUpload model)
        {
           
            if (model.files != null)
            {
                var a = await _Azure.Upload(model);
                //Console.WriteLine(a.Uri);
                return Ok(a.Uri);
            }
            return BadRequest();

            
        }

        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> Get(string fileName)
        {
            var imgBytes = await _Azure.Get(fileName);
            return File(imgBytes, "image/webp");
        }
    }
}
