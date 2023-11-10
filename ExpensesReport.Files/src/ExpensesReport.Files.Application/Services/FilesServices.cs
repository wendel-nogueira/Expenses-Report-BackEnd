using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ExpensesReport.Files.Application.Exceptions;
using ExpensesReport.Files.Application.ViewModels;
using Microsoft.AspNetCore.Http;

namespace ExpensesReport.Files.Application.Services
{
    public class FilesServices : IFilesServices
    {
        private readonly BlobContainerClient _containerClient;

        public FilesServices(string storageAccount, string key, string container)
        {
            var credential = new StorageSharedKeyCredential(storageAccount, key);
            var blobServiceClient = new BlobServiceClient(new Uri($"https://{storageAccount}.blob.core.windows.net"), credential);
            _containerClient = blobServiceClient.GetBlobContainerClient(container);
        }

        public async Task<IEnumerable<FileViewModel>> GetFiles()
        {
            List<FileViewModel> files = new List<FileViewModel>();

            await foreach (var blobItem in _containerClient.GetBlobsAsync())
            {
                var blobClient = _containerClient.GetBlobClient(blobItem.Name);

                files.Add(new FileViewModel
                {
                    Name = blobClient.Name,
                    Uri = blobClient.Uri.AbsoluteUri,
                    ContentType = blobClient.GetProperties().Value.ContentType,
                    Size = blobClient.GetProperties().Value.ContentLength,
                });
            }

            return files;
        }

        public async Task<FileViewModel> GetFileByName(string name)
        {
            var blobClient = _containerClient.GetBlobClient(name);

            if (!await blobClient.ExistsAsync())
                throw new BadRequestException("File not found", []);

            var file = new FileViewModel
            {
                Name = blobClient.Name,
                Uri = blobClient.Uri.AbsoluteUri,
                ContentType = blobClient.GetProperties().Value.ContentType,
                Size = blobClient.GetProperties().Value.ContentLength,
            };

            return file;
        }

        public async Task<FileViewModel> UploadFile(IFormFile file)
        {
            var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = _containerClient.GetBlobClient(newFileName);

            await blobClient.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders { ContentType = file.ContentType });

            return new FileViewModel
            {
                Name = blobClient.Name,
                Uri = blobClient.Uri.AbsoluteUri,
                ContentType = blobClient.GetProperties().Value.ContentType,
                Size = blobClient.GetProperties().Value.ContentLength,
            };
        }

        public async Task DeleteFile(string name)
        {
            var blobClient = _containerClient.GetBlobClient(name);

            if (!await blobClient.ExistsAsync())
                throw new BadRequestException("File not found", []);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}
