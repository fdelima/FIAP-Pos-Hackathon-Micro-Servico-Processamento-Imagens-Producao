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
            // Configuração do ambiente de teste
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { $"ConnectionStrings:{Constants.AZ_STORAGE_CONN_NAME}", "UseDevelopmentStorage=true" }, // String de conexão para o emulador
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
            var queueClient = new QueueClient("UseDevelopmentStorage=true", _queueProcessedName);
            await queueClient.SendMessageAsync(messageBody);

            // Act
            var receivedMessage = await _messagerGateway.ReceiveMessageAsync();
            Assert.NotNull(receivedMessage); // Garante que a mensagem foi recebida
            Assert.Equal(messageBody, receivedMessage.MessageText);

            // Act
            await _messagerGateway.DeleteMessageAsync(receivedMessage);

            // Assert
            var messageDeleted = await queueClient.ReceiveMessagesAsync();
            Assert.True(messageDeleted.Value.All(m => m.MessageId != receivedMessage.MessageId));
        }
    }
}