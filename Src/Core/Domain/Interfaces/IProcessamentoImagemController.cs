using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces
{
    /// <summary>
    /// Interface regulamentando os métodos que precisam ser impementados pelos serviços da aplicação
    /// </summary>
    public interface IProcessamentoImagemController
    {
        /// <summary>
        /// Lê as mensagens dos arquivos processados.
        /// </summary>
        Task<ModelResult> ReceiverMessageInQueueAsync();
    }
}
