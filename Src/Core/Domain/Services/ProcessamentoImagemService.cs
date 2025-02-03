using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Messages;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using FluentValidation;
using System.Linq.Expressions;
using System.Text.Json;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Services
{
    public class ProcessamentoImagemService : BaseService<ProcessamentoImagem>, IProcessamentoImagemService
    {
        protected readonly IGateways<Notificacao> _notificacaoGateway;
        protected readonly IMessagerService _messagerService;

        /// <summary>
        /// Lógica de negócio referentes ao pedido.
        /// </summary>
        /// <param name="gateway">Gateway de pedido a ser injetado durante a execução</param>
        /// <param name="validator">abstração do validador a ser injetado durante a execução</param>
        /// <param name="notificacaoGateway">Gateway de notificação a ser injetado durante a execução</param>
        /// <param name="MercadoPagoWebhoockGateway">Gateway de MercadoPagoWebhoock a ser injetado durante a execução</param>
        public ProcessamentoImagemService(IGateways<ProcessamentoImagem> gateway,
            IValidator<ProcessamentoImagem> validator,
            IGateways<Notificacao> notificacaoGateway,
            IMessagerService messagerService)
            : base(gateway, validator)
        {
            _notificacaoGateway = notificacaoGateway;
            _messagerService = messagerService;
        }

        /// <summary>
        /// Regras para inserção do processamento de imagem
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="ValidatorResult">Validações já realizadas a serem adicionadas ao contexto</param>
        public override async Task<ModelResult> InsertAsync(ProcessamentoImagem entity, string[]? businessRules = null)
        {
            entity.IdProcessamentoImagem = entity.IdProcessamentoImagem.Equals(default) ? Guid.NewGuid() : entity.IdProcessamentoImagem;
            entity.Data = DateTime.Now;

            ModelResult ValidatorResult = await ValidateAsync(entity);

            Expression<Func<IDomainEntity, bool>> duplicatedExpression = entity.InsertDuplicatedRule();

            if (ValidatorResult.IsValid)
            {
                if (duplicatedExpression != null)
                {
                    bool duplicado = await _gateway.Any(duplicatedExpression);

                    if (duplicado)
                        ValidatorResult.Add(ModelResultFactory.DuplicatedResult<ProcessamentoImagem>());
                }

                if (businessRules != null)
                    ValidatorResult.AddError(businessRules);

                if (!ValidatorResult.IsValid)
                    return ValidatorResult;

                await _gateway.InsertAsync(entity);
                await _gateway.CommitAsync();
                await _notificacaoGateway.InsertAsync(new Notificacao
                {
                    Mensagem = "Arquivo recebido com sucesso!",
                    Usuario = entity.Usuario
                });
                await _notificacaoGateway.CommitAsync();
                return ModelResultFactory.InsertSucessResult<ProcessamentoImagem>(entity);
            }

            return ValidatorResult;
        }

        /// <summary>
        /// Lê as mensagens dos arquivos processados.
        /// </summary>
        public async Task<ModelResult> ReceiverMessageInQueueAsync()
        {
            var result = new ModelResult();

            var messagesBody = await _messagerService.ReceiveMessagesAsync();

            foreach (var body in messagesBody)
            {
                var msg = JsonSerializer.Deserialize<ProcessamentoImagemProcessModel>(body);

                if (msg == null)
                {
                    result.AddError(BusinessMessages.NotFoundError<ProcessamentoImagem>());
                }
                else
                {
                    try
                    {

                        var item = await _gateway.FindByIdAsync(msg.IdProcessamentoImagem);
                        if (item == null)
                        {
                            result.AddError(BusinessMessages.NotFoundInError<ProcessamentoImagem>(msg.IdProcessamentoImagem));
                            return result;
                        }

                        item.DataInicioProcessamento = msg.DataInicioProcessamento;

                        if (msg.DataFimProcessamento != null)
                            item.DataFimProcessamento = msg.DataFimProcessamento;

                        await _gateway.UpdateAsync(item);
                        result.AddMessage(ModelResultFactory.UpdateSucessResult<ProcessamentoImagem>(item).Messages);
                    }
                    catch (Exception ex)
                    {
                        result.AddError($"{body} {ex.Message}");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Envia as mensagens dos arquivos recebidos para a fila.
        /// </summary>
        public async Task<ModelResult> SendMessageToQueueAsync()
        {
            var result = new ModelResult();

            var ItemsToSendPage = await _gateway.GetItemsAsync(x => x.DataEnviadoFila == null);

            foreach (var item in ItemsToSendPage.Content)
            {
                var msg = new ProcessamentoImagemSendQueueModel
                {
                    IdProcessamentoImagem = item.IdProcessamentoImagem,
                    Usuario = item.Usuario,
                    DataEnviadoFila = DateTime.Now,
                    NomeArquivo = item.NomeArquivo,
                    TamanhoArquivo = item.TamanhoArquivo,
                    NomeArquivoZipDownload = item.NomeArquivoZipDownload
                };

                var sendMessageTask = _messagerService.SendMessageAsync(JsonSerializer.Serialize(msg));
                await sendMessageTask;

                if (sendMessageTask.IsCompletedSuccessfully)
                {
                    var entity = await _gateway.FindByIdAsync(item.IdProcessamentoImagem);
                    entity.DataEnviadoFila = msg.DataEnviadoFila;
                    await _gateway.UpdateAsync(entity);
                    result.AddMessage($"{item.IdProcessamentoImagem.ToString()} :: Send Message To Queue Success!");
                }
                else
                    result.AddError($"{item.IdProcessamentoImagem.ToString()} :: Send Message To Queue Failed! {sendMessageTask.Exception?.Message ?? ""}");
            }

            return result;
        }
    }
}