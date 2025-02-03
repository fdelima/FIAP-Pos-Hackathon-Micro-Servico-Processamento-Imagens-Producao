
namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces
{
    public interface IMessagerService
    {
        Task<string> ReceiveMessagesAsync();
        Task SendMessageAsync(string messageBody);
    }
}