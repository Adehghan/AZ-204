
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Storage.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UploadFileController : ControllerBase
    {

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("No file is uploaded.");

            // 1- Create connection
            string connectionString = "YOUR STORAGE ACCOUNT CONNECTION";
            if (string.IsNullOrWhiteSpace(connectionString))
                return StatusCode(500, "Storage connection string is missing. Set AZURE_STORAGE_CONNECTION_STRING.");

            // 2- Client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containername = "test" + Guid.NewGuid().ToString();
            var containerClient = blobServiceClient.GetBlobContainerClient(containername);

            // Create container if it doesn't exist (idempotent)
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            // 3) Target blob
            var blobClient = containerClient.GetBlobClient(file.FileName);

            // 4) Upload stream (overwrite true if you want to replace same-name files)
            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            // Optional: return blob URL
            return Ok(new
            {
                file = file.FileName,
                url = blobClient.Uri.ToString()
            });
        }
    }
}