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
        internal readonly WorkerTestFixture _workerTest;
        private readonly IStorageGateway _storageGateway;
        private readonly IMessagerGateway _messagerGateway;
        private ProcessamentoImagemSendQueueModel _msgSendModel;
        private readonly string _queueToProcessName = "processar";
        private readonly string _queueProcessedName = "processado";
        private readonly string _conn = "UseDevelopmentStorage=true";

        /// <summary>
        /// Construtor da classe de teste.
        /// </summary>
        public ProcessamentoImagemControllerTest(ComponentTestsBase data)
        {
            _workerTest = data._workerTest;
            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { $"ConnectionStrings:{Constants.AZ_STORAGE_CONN_NAME}", _conn },
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
            var queueClient = new QueueClient(_conn, _queueToProcessName);
            await _messagerGateway.SendMessageAsync(messageBody);

            //Aguardando o worker fazer o trabalho
            Thread.Sleep(1000 * 45);

            Assert.True(FileExistsInStorageAsync(Constants.BLOB_CONTAINER_NAME, _msgSendModel.NomeArquivoZipDownload));
        }

        private bool FileExistsInStorageAsync(string containerName, string fileName)
        {
            var blobServiceClient = new BlobServiceClient(_conn);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            return blobClient != null;
        }


        [Then(@"Encerra o trabalho")]
        public async Task KillWorker()
        {
            _workerTest.Dispose();

            Assert.True(true);
        }
    }
}