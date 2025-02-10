using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Interfaces;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Models;
using FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Services;
using NSubstitute;

namespace TestProject.UnitTest.Domain
{
    public class MessageServiceTest
    {
        private readonly IMessagerGateway _messagerGatewaySubstitute;
        private readonly MessageService _messageService;

        public MessageServiceTest()
        {
            _messagerGatewaySubstitute = Substitute.For<IMessagerGateway>();
            _messageService = new MessageService(_messagerGatewaySubstitute);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldCallGateway()
        {
            // Arrange
            var messageBody = "Test message";

            // Act
            await _messageService.SendMessageAsync(messageBody);

            // Assert
            await _messagerGatewaySubstitute.Received(1).SendMessageAsync(messageBody);
        }

        [Fact]
        public async Task ReceiveMessageAsync_ShouldCallGateway()
        {
            // Arrange
            var messageModel = new MessageModel
            {
                MessageId = "1",
                PopReceipt = "Test receipt",
                MessageText = "Test message"
            };

            _messagerGatewaySubstitute.ReceiveMessageAsync().Returns(Task.FromResult<MessageModel?>(messageModel));

            // Act
            var result = await _messageService.ReceiveMessageAsync();

            // Assert
            await _messagerGatewaySubstitute.Received(1).ReceiveMessageAsync();
            Assert.NotNull(result);
            Assert.Equal(messageModel.MessageId, result.MessageId);
        }

        [Fact]
        public async Task DeleteMessageAsync_ShouldCallGateway()
        {
            // Arrange
            var messageModel = new MessageModel
            {
                MessageId = "1",
                PopReceipt = "Test receipt",
                MessageText = "Test message"
            };

            // Act
            await _messageService.DeleteMessageAsync(messageModel);

            // Assert
            await _messagerGatewaySubstitute.Received(1).DeleteMessageAsync(messageModel);
        }
    }
}