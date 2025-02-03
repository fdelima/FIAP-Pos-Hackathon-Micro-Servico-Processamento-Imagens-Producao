using Azure.Storage.Blobs;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.Gateways
{
    public class AzureBlobStorageGateway : IStorageGateway
    {
        private readonly string _connectionString;
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobStorageGateway(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Constants.AZ_STORAGE_CONN_NAME) ?? "";
            _blobServiceClient = new BlobServiceClient(_connectionString);
        }

        public async Task UploadFileAsync(string containerName, string fileName, Stream fileStream)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);
            fileStream.Position = 0;
            await blobClient.UploadAsync(fileStream, true);
        }

        public async Task DownloadFileAsync(string containerName, string fileName, string localFilePath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.DownloadToAsync(localFilePath);
        }

        public async Task DeleteFileAsync(string containerName, string fileName)
        {
            // Obtém o cliente do contêiner
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            // Obtém o cliente do blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Exclui o blob
            await blobClient.DeleteAsync();
        }
    }
}
