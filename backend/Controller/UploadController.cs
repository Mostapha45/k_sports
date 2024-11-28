using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController(BlobServiceClient blobServiceClient) : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var containerClient = blobServiceClient.GetBlobContainerClient("ksports");
            var blobClient = containerClient.GetBlobClient(file.FileName);

            await using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return Ok(new { fileName = file.FileName, url = blobClient.Uri.ToString() });
        }

        [HttpPost("simulate-upload")]
        public async Task<IActionResult> SimulateUpload()
        {
            // Create a mock file
            var content = "Hello World from a Fake File";
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            var fileMock = new FormFile(ms, 0, ms.Length, "mockFile", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            return await UploadImage(fileMock);
        }
    }
}