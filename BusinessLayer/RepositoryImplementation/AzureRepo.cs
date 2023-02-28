using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BusinessLayer.Repository;
using DataAcessLayer.FormFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RepositoryImplementation;

public class AzureRepo : IAzureLogic
{

    private readonly BlobServiceClient _blobServiceClient;
    public AzureRepo(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }
    
    public async Task<BlobClient> Upload([FromForm] FileUpload file)
    {

            //var formCollection = await Request.ReadFormAsync();
            //var file = formCollection.Files.First();

            
                var container = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=rdtecommerce121;AccountKey=B0b5OdXjAplPEKu6zimtq6uxPbwl2zYO+Kaw1S4xifpVSrf8fL25gTM08Fmcgppm7lS2jbfRyr7n+AStiN3fGQ==;EndpointSuffix=core.windows.net", "hello");
                var createResponse = await container.CreateIfNotExistsAsync();
                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                    await container.SetAccessPolicyAsync(PublicAccessType.Blob);

                var blob = container.GetBlobClient(file.files.FileName);
                //await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                using (var fileStream = file.files.OpenReadStream())
                {
                    await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.files.ContentType });
                }

                return blob;
            

        
       
    }
   //public async Task<BlobClient> Upload([FromForm] FileUpload model)
   // {
   //     var blobContainer = _blobServiceClient.GetBlobContainerClient("uploadfiles");
   //      _blobServiceClient.CreateBlobContainer("hello");
       
   //     var blobClient = blobContainer.GetBlobClient(model.files.FileName);

   //     await blobClient.UploadAsync(model.files.OpenReadStream());

   //     return blobClient; 
   // }

    public async Task<byte[]> Get(string imageName)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient("hello");
      //   _blobServiceClient.CreateBlobContainer("hello");
       
        var blobClient = blobContainer.GetBlobClient(imageName);
        var downloadContent = await blobClient.DownloadAsync();
        using (MemoryStream ms = new MemoryStream())
        {
            await downloadContent.Value.Content.CopyToAsync(ms);
            return ms.ToArray();
        }
    }

    
}
