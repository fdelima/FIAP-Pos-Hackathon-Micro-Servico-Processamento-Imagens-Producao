using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.Gateways
{
    public class AzureQueueStorageGateway : IMessagerGateway
    {
        private readonly string _connectionString;
        private readonly QueueClient _queueReceiverClient;
        private readonly QueueClient _queueSenderClient;

        public AzureQueueStorageGateway(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Domain.Constants.AZ_STORAGE_CONN_NAME) ?? "";

            _queueSenderClient = new QueueClient(_connectionString, configuration[Domain.Constants.MESSAGER_QUEUE_TO_PROCESS_NAME]);
            _queueSenderClient.CreateIfNotExists();

            _queueReceiverClient = new QueueClient(_connectionString, configuration[Domain.Constants.MESSAGER_QUEUE_PROCESSED_NAME]);
            _queueReceiverClient.CreateIfNotExists();
        }

        public async Task<string> ReceiveMessagesAsync()
        {
            QueueMessage message = await _queueReceiverClient.ReceiveMessageAsync();
            return message.MessageText;
        }

        public async Task SendMessageAsync(string messageBody)
        {
            await _queueSenderClient.SendMessageAsync(messageBody);
        }
    }
}
