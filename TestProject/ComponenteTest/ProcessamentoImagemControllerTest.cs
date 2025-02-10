using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.Gateways;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;
using TestProject.Infra;
using Xunit.Gherkin.Quick;

namespace TestProject.ComponenteTest
{
    [FeatureFile("./BDD/Features/ControlarProcessamentoImagens.feature")]
    public class ProcessamentoImagemControllerTest : Feature, IClassFixture<ComponentTestsBase>
    {
        private readonly IStorageGateway _storageGateway;
        private readonly IMessagerGateway _messagerGateway;
        private ProcessamentoImagemSendQueueModel _msgSendModel;
        private readonly string _queueToProcessName = "processar";
        private readonly string _queueProcessedName = "processado";

        /// <summary>
        /// Construtor da classe de teste.
        /// </summary>
        public ProcessamentoImagemControllerTest(ComponentTestsBase data)
        {
            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { $"ConnectionStrings:{Constants.AZ_STORAGE_CONN_NAME}", "UseDevelopmentStorage=true" },
                    { Constants.MESSAGER_QUEUE_TO_PROCESS_NAME, _queueToProcessName },
                    { Constants.MESSAGER_QUEUE_PROCESSED_NAME, "processado" }
                })
                .Build();
            _storageGateway = new AzureBlobStorageGateway(configuration);
            _messagerGateway = new AzureQueueStorageGateway(configuration);
        }

        [Given(@"Prepando um ProcessamentoImagem")]
        public async Task PrepararProcessamentoImagem()
        {
            var id = Guid.NewGuid();
            var videoStream = new MemoryStream(File.ReadAllBytes("video_test.mp4"));
            await _storageGateway.UploadFileAsync(Constants.BLOB_CONTAINER_NAME, $"{id}.mp4", videoStream);

            _msgSendModel = new ProcessamentoImagemSendQueueModel
            {
                IdProcessamentoImagem = id,
                Usuario = "usuario_teste",
                NomeArquivo = "video_test.mp4",
                TamanhoArquivo = videoStream.Length,
                NomeArquivoZipDownload = $"{id}.zip"
            };

        }

        [And(@"Produzir o ProcessamentoImagem")]
        public async Task SendProcessamentoImagem()
        {
            string messageBody = JsonSerializer.Serialize(_msgSendModel);
            var queueClient = new QueueClient("UseDevelopmentStorage=true", _queueToProcessName);
            await _messagerGateway.SendMessageAsync(messageBody);

            //Aguardando o worker fazer o trabalho
            Thread.Sleep(1000 * 30);

            Assert.True(await FileExistsInStorageAsync(Constants.BLOB_CONTAINER_NAME, _msgSendModel.NomeArquivoZipDownload));
        }

        private async Task<bool> FileExistsInStorageAsync(string containerName, string fileName)
        {
            var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            return await blobClient.ExistsAsync();
        }

        //[When(@"Receber o ProcessamentoImagem")]
        //public async Task ReceiverProcessamentoImagem()
        //{
        //    throw new NotImplementedException();
        //}


        //[Then(@"posso deletar o ProcessamentoImagem")]
        //public async Task DeletarProcessamentoImagem()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
