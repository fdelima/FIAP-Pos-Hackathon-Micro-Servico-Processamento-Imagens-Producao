using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Services
{
    public class MessageService : IMessagerService
    {
        IMessagerGateway _messagerGateway;

        public MessageService(IMessagerGateway messagerGateway)
        {
            _messagerGateway = messagerGateway;
        }
        public Task<string> ReceiveMessagesAsync()
        {
            return _messagerGateway.ReceiveMessagesAsync();
        }

        public Task SendMessageAsync(string messageBody)
        {
            return _messagerGateway.SendMessageAsync(messageBody);
        }
    }
}
