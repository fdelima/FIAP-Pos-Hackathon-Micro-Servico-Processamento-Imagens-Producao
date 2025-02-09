using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Commands;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Application.UseCases.ProcessamentoImagem.Handlers;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using NSubstitute;

namespace TestProject.UnitTest.Aplication
{
    //TODO: Seguir como cliente no micro serviço cadastro
    public partial class ProcessamentoImagemUseCasesTest
    {
        private readonly IProcessamentoImagemService _service;

        /// <summary>
        /// Construtor da classe de teste.
        /// </summary>
        public ProcessamentoImagemUseCasesTest()
        {
            _service = Substitute.For<IProcessamentoImagemService>();
        }

        [Fact]
        public async Task ReceiverMessageInQueueAsyncTest()
        {
            //Arrange
            var command = new ProcessamentoImagemReceiverMessageInQueueCommand();

            //Mockando retorno do serviço de domínio.
            _service.ReceiverMessageInQueueAsync()
                .Returns(Task.FromResult(ModelResultFactory.SucessResult()));

            //Act
            var handler = new ProcessamentoImagemReceiverMessageInQueueHandler(_service);
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.True(result.IsValid);
        }


        [Fact]
        public async Task SendMessageToQueueAsyncTest()
        {
            throw new NotImplementedException();
        }
    }
}
