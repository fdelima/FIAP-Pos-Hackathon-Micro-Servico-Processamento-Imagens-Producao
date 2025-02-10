using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Services;
using NSubstitute;

namespace TestProject.UnitTest.Domain
{
    public class AzureBlobStorageServiceTest
    {
        private readonly IStorageGateway _storageGatewaySubstitute;
        private readonly AzureBlobStorageService _service;

        public AzureBlobStorageServiceTest()
        {
            _storageGatewaySubstitute = Substitute.For<IStorageGateway>();
            _service = new AzureBlobStorageService(_storageGatewaySubstitute);
        }

        [Fact]
        public async Task UploadFileAsync_ShouldCallGateway()
        {
            // Arrange
            var fileName = "test-file.txt";
            var fileStream = new MemoryStream();
            var containerName = "test-container";

            // Act
            await _service.UploadFileAsync(containerName, fileName, fileStream);

            // Assert
            await _storageGatewaySubstitute.Received(1).UploadFileAsync(containerName, fileName, fileStream);
        }

        [Fact]
        public async Task DownloadFileAsync_ShouldCallGateway()
        {
            // Arrange
            var fileName = "test-file.txt";
            var containerName = "test-container";
            var localFilePath = "local-path.txt";

            // Act
            await _service.DownloadFileAsync(containerName, fileName, localFilePath);

            // Assert
            await _storageGatewaySubstitute.Received(1).DownloadFileAsync(containerName, fileName, localFilePath);
        }

        [Fact]
        public async Task DeleteFileAsync_ShouldCallGateway()
        {
            // Arrange
            var fileName = "test-file.txt";
            var containerName = "test-container";

            // Act
            await _service.DeleteFileAsync(containerName, fileName);

            // Assert
            await _storageGatewaySubstitute.Received(1).DeleteFileAsync(containerName, fileName);
        }
    }
}