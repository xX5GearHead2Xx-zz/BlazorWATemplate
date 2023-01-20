using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Hosting;
using System.Reflection.Metadata;

namespace BlazorWATemplate.Server.Azure
{
    public class Storage
    {
        private IConfiguration _configuration;
        public Storage(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Upload(string FileKey, byte[] Data)
        {
            try
            {
                var blobClient = new BlobContainerClient(_configuration["AzureStorage_ProductImages:ConnectionString"], _configuration["AzureStorage_ProductImages:Container"]);
                var blob = blobClient.GetBlobClient(FileKey);
                if (blob.Exists())
                {
                    await blob.DeleteAsync();
                }
                Stream stream = new MemoryStream(Data);
                await blob.UploadAsync(stream);
            }
            catch (Exception Ex)
            {
                throw new Exception("Azure > Storage > UploadImage " + Ex.Message);
            }
        }

        public async Task Delete(string FileKey)
        {
            try
            {
                var blobClient = new BlobContainerClient(_configuration["AzureStorage_ProductImages:ConnectionString"], _configuration["AzureStorage_ProductImages:Container"]);
                var blob = blobClient.GetBlobClient(FileKey);
                await blob.DeleteIfExistsAsync();
            }
            catch (Exception Ex)
            {
                throw new Exception("Models > AzureStorage >  DeleteImage" + Ex.Message);
            }
        }

        public async Task<byte[]> Download(string FileKey)
        {
            try
            {
                var blobClient = new BlobContainerClient(_configuration["AzureStorage_ProductImages:ConnectionString"], _configuration["AzureStorage_ProductImages:Container"]);
                var blob = blobClient.GetBlobClient(FileKey);
                using (var ms = new MemoryStream())
                {
                    BlobDownloadResult Result = await blob.DownloadContentAsync();
                    return Result.Content.ToArray();
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Models > AzureStorage > UploadImage " + Ex.Message);
            }
        }
    }
}
