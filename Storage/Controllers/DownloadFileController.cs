
using Microsoft.AspNetCore.Mvc;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using System.IO;

namespace Storage.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DownloadFileController : ControllerBase
{
    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetFileAsync(string fileName)
    {
        if (fileName is null || fileName.Length == 0)
            return BadRequest("Please enter a fileName.");

        string keyvaultUrl = "https://virakeyvaulttest.vault.azure.net/";
        string secretName = "StorageConnectionString";
        string containerName = "viramediacontainer";

        try
        {
            // 1- Get conncetion string from key vault
            var credential = new DefaultAzureCredential();
            var secretClient = new SecretClient(new Uri(keyvaultUrl), credential);
            KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName);

            string connectionString = secret.Value;
            if (string.IsNullOrWhiteSpace(connectionString))
                return StatusCode(500, "Storage connection string secret is empty.");

            if (!connectionString.Contains("DefaultEndpointsProtocol="))
                return StatusCode(500, "Secret is not a storage connection string (expected connection string).");

            // 2- Build clients
            var blobService = new BlobServiceClient(connectionString);
            var containerClient = blobService.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            // 3- Check existence
            if (!await blobClient.ExistsAsync())
                return NotFound("Blob not found.");

            // 4) Stream download to response
            var download = await blobClient.DownloadStreamingAsync();
            var content = download.Value.Content;// Stream
            var headers = download.Value.Details;
            var contentType = headers.ContentType ?? "application/octet-stream";

            // Optional: force download as attachment
            Response.Headers["Content-Disposition"] = $"attachment; filename=\"{fileName}\"";

            return File(content, contentType);
        }
        catch (Azure.RequestFailedException ex)
        {
            // Common causes: wrong container, bad connection string, no RBAC on Key Vault
            return StatusCode(ex.Status, $"Azure error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        // If secret is a full SAS URL (to a specific blob or container), do this instead:
        /*
        var blobClient = new BlobClient(new Uri(secret.Value)); // SAS URL
        using var fs = File.OpenWrite(localPath);
        await blobClient.DownloadToAsync(fs);
        */
    }
}