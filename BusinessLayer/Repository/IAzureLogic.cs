using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAcessLayer.FormFile;
using Azure.Storage.Blobs;

namespace BusinessLayer.Repository;

public interface IAzureLogic
{
    Task <BlobClient>Upload([FromForm]FileUpload fileUpload);

    Task<byte[]> Get(string imageName);
}
