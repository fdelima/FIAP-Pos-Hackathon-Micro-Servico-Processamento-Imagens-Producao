
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces
{
    public interface IMessagerService
    {
        Task DeleteMessageAsync(MessageModel message);
        Task<MessageModel?> ReceiveMessageAsync();
        Task SendMessageAsync(string messageBody);
    }
}