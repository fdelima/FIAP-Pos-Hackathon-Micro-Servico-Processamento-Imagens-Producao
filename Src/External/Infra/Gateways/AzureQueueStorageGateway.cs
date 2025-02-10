using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
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

            _queueSenderClient = new QueueClient(_connectionString, configuration[Domain.Constants.MESSAGER_QUEUE_PROCESSED_NAME]);
            _queueSenderClient.CreateIfNotExists();

            _queueReceiverClient = new QueueClient(_connectionString, configuration[Domain.Constants.MESSAGER_QUEUE_TO_PROCESS_NAME]);
            _queueReceiverClient.CreateIfNotExists();
        }

        public async Task DeleteMessageAsync(MessageModel message)
        {
            await _queueReceiverClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }

        public async Task<MessageModel?> ReceiveMessageAsync()
        {
            QueueMessage message = await _queueReceiverClient.ReceiveMessageAsync();

            if (message == null) return null;

            return new MessageModel
            {
                MessageId = message.MessageId,
                PopReceipt = message.PopReceipt,
                MessageText = message.MessageText
            };
        }

        public async Task SendMessageAsync(string messageBody)
        {
            await _queueSenderClient.SendMessageAsync(messageBody);
        }
    }
}
