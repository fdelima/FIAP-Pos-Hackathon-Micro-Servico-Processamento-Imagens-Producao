
namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces
{
    public interface IMessagerGateway
    {
        Task<string> ReceiveMessagesAsync();
        Task SendMessageAsync(string messageBody);
    }
}