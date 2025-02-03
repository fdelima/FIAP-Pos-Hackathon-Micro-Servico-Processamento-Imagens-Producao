using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Commands;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Entities;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Messages;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using FluentValidation;
using MediatR;
using System.Linq.Expressions;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.Controllers
{
    /// <summary>
    /// Regras da aplicação referente ao ProcessamentoImagem
    /// </summary>
    public class ProcessamentoImagemController : IProcessamentoImagemController
    {
        private readonly IMediator _mediator;
        private readonly IValidator<ProcessamentoImagem> _validator;
        private readonly IValidator<ProcessamentoImagemUploadModel> _uploadValidator;
        private readonly IStorageService _storageService;

        public ProcessamentoImagemController(IMediator mediator,
            IValidator<ProcessamentoImagem> validator,
            IStorageService IStorageService,
            IValidator<ProcessamentoImagemUploadModel> uploadValidator)
        {
            _mediator = mediator;
            _validator = validator;
            _storageService = IStorageService;
            _uploadValidator = uploadValidator;
        }

        /// <summary>
        /// Valida a entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public async Task<ModelResult> ValidateAsync(ProcessamentoImagem entity)
        {
            ModelResult ValidatorResult = new ModelResult(entity);

            FluentValidation.Results.ValidationResult validations = _validator.Validate(entity);
            if (!validations.IsValid)
            {
                ValidatorResult.AddValidations(validations);
                return ValidatorResult;
            }

            return await Task.FromResult(ValidatorResult);
        }

        /// <summary>
        /// Valida a entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public async Task<ModelResult> ValidateAsync(ProcessamentoImagemUploadModel entity)
        {
            ModelResult ValidatorResult = new ModelResult(entity);

            FluentValidation.Results.ValidationResult validations = _uploadValidator.Validate(entity);
            if (!validations.IsValid)
            {
                ValidatorResult.AddValidations(validations);
                return ValidatorResult;
            }

            return await Task.FromResult(ValidatorResult);
        }

        /// <summary>
        /// Envia a entidade para inserção ao domínio
        /// </summary>
        /// <param name="entity">Entidade</param>
        public virtual async Task<ModelResult> PostAsync(ProcessamentoImagem entity)
        {
            if (entity == null) throw new InvalidOperationException($"Necessário informar os dados de processamento de imagem");

            ModelResult ValidatorResult = await ValidateAsync(entity);

            if (ValidatorResult.IsValid)
            {
                ProcessamentoImagemPostCommand command = new(entity);
                return await _mediator.Send(command);
            }

            return ValidatorResult;
        }

        /// <summary>
        /// Envia a entidade para inserção ao domínio
        /// </summary>
        /// <param name="entity">Entidade</param>
        public virtual async Task<ModelResult> PostAsync(ProcessamentoImagemUploadModel entity)
        {
            if (entity == null) throw new InvalidOperationException($"Necessário informar os dados de processamento de imagem");

            ModelResult ValidatorResult = await ValidateAsync(entity);

            if (ValidatorResult.IsValid)
            {
                if (entity.FormFile.Length > 0)
                {
                    var idProcessamentoImagem = Guid.NewGuid();
                    var fileToUpload = $"{idProcessamentoImagem}{Path.GetExtension(entity.FormFile.FileName)}";

                    var ms = new MemoryStream();
                    await entity.FormFile.CopyToAsync(ms);

                    var uploadFileTask = _storageService.UploadFileAsync(Constants.BLOB_CONTAINER_NAME, fileToUpload, ms);
                    await uploadFileTask;

                    if (!uploadFileTask.IsCompletedSuccessfully)
                        ValidatorResult.AddError(uploadFileTask.Exception?.Message ?? "upload file");


                    var piEntity = new ProcessamentoImagem
                    {
                        IdProcessamentoImagem = idProcessamentoImagem,
                        Data = entity.Data,
                        Usuario = entity.Usuario,
                        DataEnvio = DateTime.Now,
                        NomeArquivo = entity.FormFile.FileName,
                        TamanhoArquivo = entity.FormFile.Length,
                        NomeArquivoZipDownload = $"{idProcessamentoImagem}.zip"
                    };

                    ProcessamentoImagemPostCommand command = new(piEntity);
                    var result = await _mediator.Send(command);

                    if (!result.IsValid)
                    {
                        var deleteFileTask = _storageService.DeleteFileAsync(Constants.BLOB_CONTAINER_NAME, fileToUpload);

                        if (!deleteFileTask.IsCompletedSuccessfully)
                            ValidatorResult.AddError(uploadFileTask.Exception?.Message ?? "delete file");
                    }

                    return result;
                }
                else
                    ValidatorResult.AddMessage(ValidationMessages.RequiredFieldWhithPropertyName("FormFile"));

            }

            return ValidatorResult;
        }

        /// <summary>
        /// Envia a entidade para atualização ao domínio
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="duplicatedExpression">Expressão para verificação de duplicidade.</param>
        public virtual async Task<ModelResult> PutAsync(Guid id, ProcessamentoImagem entity)
        {
            if (entity == null) throw new InvalidOperationException($"Necessário informar os dados de processamento de imagem");

            ModelResult ValidatorResult = await ValidateAsync(entity);

            if (ValidatorResult.IsValid)
            {
                ProcessamentoImagemPutCommand command = new(id, entity);
                return await _mediator.Send(command);
            }

            return ValidatorResult;
        }

        /// <summary>
        /// Envia a entidade para deleção ao domínio
        /// </summary>
        /// <param name="entity">Entidade</param>
        public virtual async Task<ModelResult> DeleteAsync(Guid id)
        {
            ProcessamentoImagemDeleteCommand command = new(id);
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Retorna a entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public virtual async Task<ModelResult> FindByIdAsync(Guid id)
        {
            ProcessamentoImagemFindByIdCommand command = new(id);
            return await _mediator.Send(command);
        }


        /// <summary>
        /// Retorna as entidades
        /// </summary>
        /// <param name="filter">filtro a ser aplicado</param>
        public virtual async ValueTask<PagingQueryResult<ProcessamentoImagem>> GetItemsAsync(IPagingQueryParam filter, Expression<Func<ProcessamentoImagem, object>> sortProp)
        {
            if (filter == null) throw new InvalidOperationException("Necessário informar o filtro da consulta");

            ProcessamentoImagemGetItemsCommand command = new(filter, sortProp);
            return await _mediator.Send(command);
        }


        /// <summary>
        /// Retorna as entidades que atendem a expressão de filtro 
        /// </summary>
        /// <param name="expression">Condição que filtra os itens a serem retornados</param>
        /// <param name="filter">filtro a ser aplicado</param>
        public virtual async ValueTask<PagingQueryResult<ProcessamentoImagem>> ConsultItemsAsync(IPagingQueryParam filter, Expression<Func<ProcessamentoImagem, bool>> expression, Expression<Func<ProcessamentoImagem, object>> sortProp)
        {
            if (filter == null) throw new InvalidOperationException("Necessário informar o filtro da consulta");

            ProcessamentoImagemGetItemsCommand command = new(filter, expression, sortProp);
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Envia as mensagens dos arquivos recebidos para a fila.
        /// </summary>
        public async Task<ModelResult> SendMessageToQueueAsync()
        {
            ProcessamentoImagemSendMessageToQueueCommand command = new();
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Lê as mensagens dos arquivos processados.
        /// </summary>
        public async Task<ModelResult> ReceiverMessageInQueueAsync()
        {
            ProcessamentoImagemReceiverMessageInQueueCommand command = new();
            return await _mediator.Send(command);
        }
    }
}
