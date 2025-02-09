using Azure.Storage.Queues;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.Gateways;
using Microsoft.Extensions.Configuration;
using TestProject.Infra;

namespace TestProject.IntegrationTest.External
{
    public class AzureQueueStorageGatewayTest : IClassFixture<IntegrationTestsBase>
    {
        private readonly IMessagerGateway _messagerGateway;
        private readonly string _queueToProcessName;
        private readonly string _queueProcessedName;

        public AzureQueueStorageGatewayTest(IntegrationTestsBase data)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { $"ConnectionStrings:{Constants.AZ_STORAGE_CONN_NAME}", "UseDevelopmentStorage=true" },
                    { Constants.MESSAGER_QUEUE_TO_PROCESS_NAME, "queue-to-process" },
                    { Constants.MESSAGER_QUEUE_PROCESSED_NAME, "queue-processed" }
                })
                .Build();

            _messagerGateway = new AzureQueueStorageGateway(configuration);

            _queueToProcessName = configuration[Constants.MESSAGER_QUEUE_TO_PROCESS_NAME];
            _queueProcessedName = configuration[Constants.MESSAGER_QUEUE_PROCESSED_NAME];
        }

        [Fact]
        public async Task SendMessageAsync_ShouldSendMessageSuccessfully()
        {
            // Arrange
            string messageBody = "Test message";

            // Act
            await _messagerGateway.SendMessageAsync(messageBody);

            // Assert
            var queueClient = new QueueClient("UseDevelopmentStorage=true", _queueToProcessName);
            var messages = await queueClient.ReceiveMessagesAsync();
            Assert.True(messages.Value.Length > 0);
            Assert.Equal(messageBody, messages.Value[0].MessageText);
        }

        [Fact]
        public async Task ReceiveMessageAsync_ShouldReceiveMessageSuccessfully()
        {
            // Arrange
            string messageBody = "Test message";
            var queueClient = new QueueClient("UseDevelopmentStorage=true", _queueProcessedName);
            await queueClient.SendMessageAsync(messageBody);

            // Act
            var receivedMessage = await _messagerGateway.ReceiveMessagesAsync();

            // Assert
            Assert.NotNull(receivedMessage);
            Assert.Equal(messageBody, receivedMessage.MessageText);
        }

        [Fact]
        public async Task DeleteMessageAsync_ShouldDeleteMessageSuccessfully()
        {
            // Arrange
            string messageBody = "Test message";
            var queueClient = new QueueClient("UseDevelopmentStorage=true", _queueProcessedName);
            await queueClient.SendMessageAsync(messageBody);

            // Act
            var receivedMessage = await _messagerGateway.ReceiveMessagesAsync();
            Assert.NotNull(receivedMessage);
            Assert.Equal(messageBody, receivedMessage.MessageText);

            // Act
            await _messagerGateway.DeleteMessageAsync(receivedMessage);

            // Assert
            var messageDeleted = await queueClient.ReceiveMessagesAsync();
            Assert.True(messageDeleted.Value.All(m => m.MessageId != receivedMessage.MessageId));
        }
    }
}