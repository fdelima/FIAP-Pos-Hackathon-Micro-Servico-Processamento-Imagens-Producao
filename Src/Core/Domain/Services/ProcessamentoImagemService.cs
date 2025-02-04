using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using System.Text.Json;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Services
{
    public class ProcessamentoImagemService : IProcessamentoImagemService
    {
        protected readonly IMessagerService _messagerService;

        /// <summary>
        /// Lógica de negócio referentes ao pedido.
        /// </summary>
        public ProcessamentoImagemService(IMessagerService messagerService)
        {
            _messagerService = messagerService;
        }

        /// <summary>
        /// Lê as mensagens dos arquivos processados.
        /// </summary>
        public async Task<ModelResult> ReceiverMessageInQueueAsync()
        {
            var result = ModelResultFactory.SucessResult();

            var message = await _messagerService.ReceiveMessageAsync();

            try
            {
                result = ModelResultFactory.SucessResult();
                if (message != null)
                {
                    var msgReceive = JsonSerializer.Deserialize<ProcessamentoImagemSendQueueModel>(message.MessageText);

                    //inicio do processamento
                    var msg = new ProcessamentoImagemProcessModel
                    {
                        IdProcessamentoImagem = msgReceive.IdProcessamentoImagem,
                        Usuario = msgReceive.Usuario,
                        DataInicioProcessamento = DateTime.Now,
                        NomeArquivo = msgReceive.NomeArquivo,
                        TamanhoArquivo = msgReceive.TamanhoArquivo,
                        NomeArquivoZipDownload = msgReceive.NomeArquivoZipDownload
                    };

                    //TODO: iniciar trabalho
                    Thread.Sleep(3000);

                    //Fim do processamento
                    msg.DataFimProcessamento = DateTime.Now;
                    await SendMessageToQueueAsync(msg);
                    await _messagerService.DeleteMessageAsync(message);

                    result = ModelResultFactory.SucessResult(msgReceive);
                }
            }
            catch (Exception ex)
            {
                result.AddError($"{message.MessageText} {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Envia as mensagens dos arquivos recebidos para a fila.
        /// </summary>
        public async Task<ModelResult> SendMessageToQueueAsync(ProcessamentoImagemProcessModel msg)
        {
            await _messagerService.SendMessageAsync(JsonSerializer.Serialize(msg));
            return ModelResultFactory.SucessResult(msg);
        }
    }
}