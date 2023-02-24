using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using System.Web;
using Microsoft.Web.Helpers;
using BusinessLayer.Repository;

namespace ECommerce.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly Interface1 _interface1;

        public ImageController(IWebHostEnvironment webHostEnvironment,Interface1 interface1)
        {
            _interface1 = interface1;
            
            _webHostEnvironment = webHostEnvironment;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
         public async Task<string> Post([FromForm] FileUpload fileUpload,int ProductID)
        {
            try
            {
               
                if(fileUpload.files.Length > 0 )
                {
                    var pth = "";
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);

                    }
                    using (FileStream fileStream = System.IO.File.Create(path + fileUpload.files.FileName))
                    {
                        
                        fileUpload.files.CopyTo(fileStream);
                        fileStream.Flush();
                        pth = path+fileUpload.files.FileName;
                        _interface1.AddImage( fileUpload.files.FileName,pth,ProductID);
                    //    Console.WriteLine(pth);
                        return "Upload Done";
                        
                    }
                }
                else
                {
                    return "failed";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            string path = _interface1.GetPath(id);
            if (System.IO.File.Exists(path))
            {
                byte[] b = System.IO.File.ReadAllBytes(path);
                return File(b,"image/png");
            }
            return null;
        }

    }
}
