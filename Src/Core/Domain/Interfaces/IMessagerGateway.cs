
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces
{
    public interface IMessagerGateway
    {
        Task<MessageModel?> ReceiveMessageAsync();
        Task SendMessageAsync(string messageBody);
        Task DeleteMessageAsync(MessageModel message);
    }
}