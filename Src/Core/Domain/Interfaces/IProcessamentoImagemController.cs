using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces
{
    /// <summary>
    /// Interface regulamentando os métodos que precisam ser impementados pelos serviços da aplicação
    /// </summary>
    public interface IProcessamentoImagemController : IController<ProcessamentoImagem>
    {
        /// <summary>
        /// Insere o objeto
        /// </summary>
        /// <param name="entity">Objeto relacional do bd mapeado</param>
        Task<ModelResult> PostAsync(ProcessamentoImagemUploadModel entity);

        /// <summary>
        /// Lê as mensagens dos arquivos processados.
        /// </summary>
        Task<ModelResult> ReceiverMessageInQueueAsync();

        /// <summary>
        /// Envia as mensagens dos arquivos recebidos para a fila.
        /// </summary>
        Task<ModelResult> SendMessageToQueueAsync();
    }
}
