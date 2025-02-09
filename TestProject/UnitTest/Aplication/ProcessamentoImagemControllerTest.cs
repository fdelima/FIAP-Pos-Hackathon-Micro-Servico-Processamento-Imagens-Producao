using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Commands;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Validator;
using FluentValidation;
using MediatR;
using NSubstitute;

namespace TestProject.UnitTest.Aplication
{
    /// <summary>
    /// Classe de teste.
    /// </summary>
    internal class ProcessamentoImagemControllerTest
    {
        private readonly IMediator _mediator;
        private readonly IStorageService _storageService;
        private readonly IValidator<ProcessamentoImagemUploadModel> _validator;

        /// <summary>
        /// Construtor da classe de teste.
        /// </summary>
        public ProcessamentoImagemControllerTest()
        {
            _mediator = Substitute.For<IMediator>();
            _storageService = Substitute.For<IStorageService>();
            _validator = new ProcessamentoImagemUploadModelValidator();
        }

        /// <summary>
        /// Valida a entidade
        /// </summary>
        /// <param name="entity">Entidade</param>
        public async Task<ModelResult> ValidateAsync(ProcessamentoImagemUploadModel entity)
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
        /// Lê as mensagens dos arquivos processados.
        /// </summary>
        public async Task<ModelResult> ReceiverMessageInQueueAsync()
        {
            ProcessamentoImagemReceiverMessageInQueueCommand command = new();
            return await _mediator.Send(command);
        }
    }
}
