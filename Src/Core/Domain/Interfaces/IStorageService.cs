
namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces
{
    public interface IStorageService
    {
        Task DeleteFileAsync(string containerName, string fileName);
        Task DownloadFileAsync(string containerName, string fileName, string localFilePath);
        Task UploadFileAsync(string containerName, string fileName, Stream fileStream);
    }
}