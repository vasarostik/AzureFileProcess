using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace Demo.Web.Controllers
{
    [Route("api/1")]
    public class UploadController : Controller
    {
        byte[] fileBytes;
        String name;

        [HttpPost]

        public async Task<IActionResult> Post([FromRoute]Guid id, [FromForm]IFormFile body, [FromForm]String email)
        {
            using (var memoryStream = new MemoryStream())
            {
                await body.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            name = email + "_" + Guid.NewGuid().ToString("n") + ".file";


            await CreateBlob(name, fileBytes);
            var filename = body.FileName;
            var contentType = body.ContentType;
            return Ok(body.FileName);
        }

        private async static Task CreateBlob(string name, byte[] data)
        {
           
            CloudStorageAccount storageAccount;
            CloudBlobClient client;
            CloudBlobContainer container;
            CloudBlockBlob blob;

            String strorageconn = Environment.GetEnvironmentVariable("CONNECT_STR");
        
            storageAccount = CloudStorageAccount.Parse(strorageconn);

            client = storageAccount.CreateCloudBlobClient();

            container = client.GetContainerReference("azure-webjobs-hosts");

            await container.CreateIfNotExistsAsync();

            blob = container.GetBlockBlobReference(name);
            //blob.Properties.ContentType = "application/json";

            using (Stream stream = new MemoryStream(data))
            {
                Console.WriteLine(stream.ToString());
                await blob.UploadFromStreamAsync(stream);
            }
        }
    }
}