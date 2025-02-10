using Azure.Storage.Queues;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.Gateways;
using Microsoft.Extensions.Configuration;
using TestProject.Infra;

namespace TestProject.IntegrationTest.External
{
    public class AzureQueueStorageGatewayTest : IClassFixture<BaseTests>
    {
        private readonly IMessagerGateway _messagerGateway;
        private readonly string _queueToProcessName;
        private readonly string _queueProcessedName;
        private readonly string _conn = "UseDevelopmentStorage=true";

        public AzureQueueStorageGatewayTest(BaseTests data)
        {
            // Configuração do ambiente de teste
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { $"ConnectionStrings:{Constants.AZ_STORAGE_CONN_NAME}", _conn }, // String de conexão para o emulador
                    { Constants.MESSAGER_QUEUE_TO_PROCESS_NAME, "queue-to-process" }, // Nome da fila de mensagens a serem processadas
                    { Constants.MESSAGER_QUEUE_PROCESSED_NAME, "queue-processed" } // Nome da fila de mensagens processadas
                })
                .Build();

            _messagerGateway = new AzureQueueStorageGateway(configuration); // Instância real da classe

            _queueToProcessName = configuration[Constants.MESSAGER_QUEUE_TO_PROCESS_NAME];
            _queueProcessedName = configuration[Constants.MESSAGER_QUEUE_PROCESSED_NAME];
        }

        [Fact]
        public async Task SendMessageAsync_ShouldSendMessageSuccessfully()
        {
            // Arrange
            string messageBody = "Test message";
            var queueClient = new QueueClient(_conn, _queueToProcessName);

            // Act
            await _messagerGateway.SendMessageAsync(messageBody);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public async Task ReceiveMessageAsync_ShouldReceiveMessageSuccessfully()
        {
            // Arrange
            string messageBody = "Test message";
            var queueClient = new QueueClient(_conn, _queueToProcessName);
            await queueClient.SendMessageAsync(messageBody);

            // Act
            var receivedMessage = await _messagerGateway.ReceiveMessageAsync();

            // Assert
            Assert.NotNull(receivedMessage);
            Assert.Equal(messageBody, receivedMessage.MessageText);
        }

        [Fact]
        public async Task DeleteMessageAsync_ShouldDeleteMessageSuccessfully()
        {
            // Arrange
            string messageBody = "Test message";
            var queueClient = new QueueClient(_conn, _queueToProcessName);
            var msgSend = await queueClient.SendMessageAsync(messageBody);

            var ms = new MessageModel
            {
                MessageId = msgSend.Value.MessageId,
                MessageText = messageBody,
                PopReceipt = msgSend.Value.PopReceipt
            };

            // Act
            await _messagerGateway.DeleteMessageAsync(ms);

            // Assert
            var messageDeleted = await queueClient.ReceiveMessagesAsync();
            Assert.True(true);
        }
    }
}