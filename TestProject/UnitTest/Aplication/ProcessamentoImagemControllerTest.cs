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
    public partial class ProcessamentoImagemControllerTest
    {
        private readonly IMediator _mediator;
        private readonly IValidator<ProcessamentoImagemUploadModel> _uploadValidator;
        private readonly IStorageService _storageService;

        public ProcessamentoImagemControllerTest()
        {
            _mediator = Substitute.For<IMediator>(); ;
            _storageService = Substitute.For<IStorageService>(); ;
            _uploadValidator = new ProcessamentoImagemUploadModelValidator();
        }

        [Fact]
        public async Task ReceiverMessageInQueueAsyncTest()
        {
            throw new NotImplementedException();
        }
    }
}

