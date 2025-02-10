using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Services;
using NSubstitute;
using System.Text.Json;

namespace TestProject.UnitTest.Domain
{
    public class ProcessamentoImagemServiceTest
    {
        private readonly IMessagerService _messagerService;
        private readonly IStorageService _storageService;
        private readonly ProcessamentoImagemService _service;

        public ProcessamentoImagemServiceTest()
        {
            _messagerService = Substitute.For<IMessagerService>();
            _storageService = Substitute.For<IStorageService>();
            _service = new ProcessamentoImagemService(_messagerService, _storageService, true);
        }

        [Fact]
        public async Task ReceiverMessageInQueueAsync_ComDadosValidos()
        {
            // Arrange
            var msgModel = new ProcessamentoImagemSendQueueModel
            {
                IdProcessamentoImagem = Guid.NewGuid(),
                Usuario = "usuario_teste",
                NomeArquivo = "video_test.mp4",
                TamanhoArquivo = 1024,
                NomeArquivoZipDownload = "frames.zip"
            };

            var message = new MessageModel
            {
                MessageText = JsonSerializer.Serialize(msgModel)
            };

            //MOCK
            _messagerService.ReceiveMessageAsync().Returns(Task.FromResult(message));
            _messagerService.DeleteMessageAsync(Arg.Any<MessageModel>()).Returns(Task.CompletedTask);
            _storageService.DownloadFileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Task.CompletedTask);
            _storageService.UploadFileAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Stream>()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.ReceiverMessageInQueueAsync();

            Console.WriteLine("Lima");
            foreach (var item in result.Errors)
            {
                Console.WriteLine(item);
            }

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ReceiverMessageInQueueAsync_ComDadosInvalidos()
        {
            // Arrange
            var message = new MessageModel { MessageText = "invalid-json" };
            _messagerService.ReceiveMessageAsync().Returns(Task.FromResult(message));

            // Act
            var result = await _service.ReceiverMessageInQueueAsync();

            // Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public async Task SendMessageToQueueAsync_ComDadosValidos()
        {
            // Arrange
            var msg = new ProcessamentoImagemProcessModel
            {
                IdProcessamentoImagem = Guid.NewGuid(),
                Usuario = "usuario_teste",
                NomeArquivo = "video.mp4",
                TamanhoArquivo = 1024,
                NomeArquivoZipDownload = "frames.zip"
            };

            _messagerService.SendMessageAsync(Arg.Any<string>()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.SendMessageToQueueAsync(msg);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
