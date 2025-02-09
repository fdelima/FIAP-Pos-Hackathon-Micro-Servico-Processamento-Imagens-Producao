using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Services
{
    public class MessageService : IMessagerService
    {
        IMessagerGateway _messagerGateway;

        public MessageService(IMessagerGateway messagerGateway)
        {
            _messagerGateway = messagerGateway;
        }

        public Task DeleteMessageAsync(MessageModel message)
        {
            return _messagerGateway.DeleteMessageAsync(message);
        }

        public Task<MessageModel?> ReceiveMessageAsync()
        {
            return _messagerGateway.ReceiveMessagesAsync();
        }

        public Task SendMessageAsync(string messageBody)
        {
            return _messagerGateway.SendMessageAsync(messageBody);
        }
    }
}
