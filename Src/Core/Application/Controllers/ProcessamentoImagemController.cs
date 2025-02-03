using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Commands;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using FluentValidation;
using MediatR;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.Controllers
{
    /// <summary>
    /// Regras da aplicação referente ao ProcessamentoImagem
    /// </summary>
    public class ProcessamentoImagemController : IProcessamentoImagemController
    {
        private readonly IMediator _mediator;
        private readonly IValidator<ProcessamentoImagemUploadModel> _uploadValidator;
        private readonly IStorageService _storageService;

        public ProcessamentoImagemController(IMediator mediator,
            IStorageService IStorageService,
            IValidator<ProcessamentoImagemUploadModel> uploadValidator)
        {
            _mediator = mediator;
            _storageService = IStorageService;
            _uploadValidator = uploadValidator;
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
        /// Lê as mensagens dos arquivos processados.
        /// </summary>
        public async Task<ModelResult> ReceiverMessageInQueueAsync()
        {
            ProcessamentoImagemReceiverMessageInQueueCommand command = new();
            return await _mediator.Send(command);
        }
    }
}
