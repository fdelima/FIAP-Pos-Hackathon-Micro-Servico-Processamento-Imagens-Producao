using Azure.Messaging.ServiceBus;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Infra.Gateways
{
    public class AzureServiceBusGateway : IMessagerGateway
    {
        private readonly string _connectionString;
        private readonly ServiceBusReceiver _queueReceiverClient;
        private readonly ServiceBusSender _queueSenderClient;

        public AzureServiceBusGateway(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Constants.AZ_SERVICEBUS_CONN_NAME) ?? "";
            var client = new ServiceBusClient(_connectionString);
            _queueSenderClient = client.CreateSender(configuration[Domain.Constants.MESSAGER_QUEUE_TO_PROCESS_NAME]);
            _queueReceiverClient = client.CreateReceiver(configuration[Domain.Constants.MESSAGER_QUEUE_PROCESSED_NAME]);
        }

        public async Task<string> ReceiveMessagesAsync()
        {
            var message = await _queueReceiverClient.ReceiveMessageAsync();
            return message.Body.ToString();
        }

        public async Task SendMessageAsync(string messageBody)
        {
            await _queueSenderClient.SendMessageAsync(new ServiceBusMessage(messageBody));
        }
    }
}
