using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using System.Web;

using BusinessLayer.Repository;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.Net;
using DataAcessLayer.FormFile;

namespace ECommerce.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {



        public Cloudinary cloudinary;
        public string ImagePath;
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly Interface1 _interface1;

        public ImageController(IWebHostEnvironment webHostEnvironment, Interface1 interface1)
        {
            _interface1 = interface1;

            _webHostEnvironment = webHostEnvironment;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<string> Post([FromForm] FileUpload fileUpload, int ProductID)
        {
            //try
            //{
            var file = fileUpload.files;
            var uploadresult = new ImageUploadResult();
            if (file.Length > 0)
            {
                var cloudinary = new Cloudinary(new Account("dx2q36mer", "175567385587213", "AeKizDqHyHCSLi7W08Uqq23a5zo"));
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        PublicId = file.Name,
                        Folder = "Database"

                    };


                    uploadresult = cloudinary.Upload(uploadParams);
                    _interface1.AddImage(fileUpload.files.FileName, uploadresult.Uri.ToString(), ProductID);
                    var path = uploadresult.Uri;
                }

                return "Upload Done";
            }
            else
            {
                return "failed";
            }
        }

    
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<string> Get(int id)
        {
          
            

            var path = await _interface1.GetPath(id);
            return path;

            //if (System.IO.File.Exists(path))
            //{

            //    byte[] b = System.IO.File.ReadAllBytes(path);
            //    var a = File(b, "image/png"); 

            //      // CloudinaryRepo cl = new CloudinaryRepo(, 2);

            //    return File(b, "image/png");
            //}
            //else
            //{
            //    var imageData = new WebClient().DownloadData(path);

                
            //    return File(imageData, "image/png");

            //}
      
      
             }

    }
}
